﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    private const string fileName = "data.lbg";

    private string filePath => Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName;

    private GameState _state = new GameState();

    public bool loaded = false;

    public UnityEvent ready;
    
    public UnityEvent playerInventoryChanged;

    public UnityEvent playerWearsChanged;

    public UnityEvent playerSkinChanged;

    public List<PlayerItem> items = new List<PlayerItem>();

    public List<Skin> skins = new List<Skin>();

    public List<string> PlayerInventory => _state.playerInventory;

    public List<string> PlayerWears => _state.playerWears;

    public int HighScore => _state.highScore;

    public int Coins => _state.coins;

    public string PlayerSkinId => String.IsNullOrEmpty(_state.playerSkinId) ? GameState.DefaultPlayerSkin : _state.playerSkinId;

    public string PlayerToken => _state.token;
    
    public string PlayerId => _state.lbgPlayerId;
    
    public string PlayerNickname => String.IsNullOrEmpty(_state.nickname) ? "Player-" + Random.Range(0, 100) : _state.nickname;

    public List<Sound.SoundType> MutedSounds => _state.mutedSounds ?? new List<Sound.SoundType>();

    //API
    public string apiUrl = "";
    public string gameToken = "";

    private ApiService _service;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _service = new ApiService(apiUrl, gameToken);
            DontDestroyOnLoad(this);
            Load();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (loaded)
        {
            playerInventoryChanged.Invoke();
            playerWearsChanged.Invoke();
        }
    }

    void Load()
    {
        if (loaded)
        {
            return;
        }

        if (playerInventoryChanged == null)
        {
            playerInventoryChanged = new UnityEvent();
        }

        if (playerWearsChanged == null)
        {
            playerWearsChanged = new UnityEvent();
        }
        
        if (ready == null)
        {
            ready = new UnityEvent();
        }

        if (File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            GameState state = (GameState)binaryFormatter.Deserialize(file);
            file.Close();

            _state = state;
            
            ready.Invoke();
        }

        if (IsLoggedIn())
        {
            StartCoroutine(LoadFromServer());
        }
        
        loaded = true;
    }

    IEnumerator LoadFromServer()
    {
        CoroutineWithData cd = new CoroutineWithData(this, _service.Me());
        
        yield return cd.coroutine;

        if (cd.result == null)
        {
            yield break;
        }

        ApiService.PlayerRespond respond = (ApiService.PlayerRespond) cd.result;
        
        _state.lbgPlayerId = respond.playerId.ToString();
        _state.nickname = respond.nickname;

        foreach (string item in respond.inventory)
        {
            if (!_state.playerInventory.Exists(i => i == item))
            {
                _state.playerInventory.Add(item);
            }
        }
        
        foreach (string item in respond.wears)
        {
            if (!_state.playerWears.Exists(i => i == item))
            {
                _state.playerWears.Add(item);
            }
        }

        if (!String.IsNullOrEmpty(respond.skinId))
        {
            _state.playerSkinId = respond.skinId;
        }
        
        if (_state.highScore < respond.highScore)
        {
            _state.highScore = respond.highScore;
        }

        if (_state.coins < respond.coins)
        {
            _state.coins = respond.coins;
        }
        
        ready.Invoke();
        Save(false);
    }
    
    void Save(bool persistServer = true)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        binaryFormatter.Serialize(file, _state);
        file.Close();

        if (persistServer && IsLoggedIn())
        {
            ApiService.PlayerUpdateRequest request = new ApiService.PlayerUpdateRequest
            {
                coins = _state.coins,
                highScore = _state.highScore,
                nickname = _state.nickname,
                skinId = _state.playerSkinId,
                inventory = _state.playerInventory.ToArray(),
                wears = _state.playerWears.ToArray()
            };
            
            StartCoroutine(_service.Update(request));
        }
    }

    public void SetPlayerInventory(List<string> newInventory)
    {
        _state.playerInventory = newInventory;
        Save();
        playerInventoryChanged.Invoke();
    }

    public void AddItemToPlayerInventory(string itemId)
    {
        _state.playerInventory.Add(itemId);
        Save();
        playerInventoryChanged.Invoke();
    }

    public void RemoveFromPlayerWears(string itemId)
    {
        if (_state.playerWears.Exists(id => id == itemId))
        {
            _state.playerWears.Remove(itemId);
            Save();
            playerWearsChanged.Invoke();
        }
    }

    public void AddToPlayerWears(string itemId)
    {
        if (!_state.playerWears.Exists(id => id == itemId))
        {
            _state.playerWears.Add(itemId);
            Save();
            playerWearsChanged.Invoke();
        }
    }

    public void SetPlayerWears(List<string> playerWears)
    {
        _state.playerWears = playerWears;
        Save();
        playerWearsChanged.Invoke();
    }

    public void SetHighScore(int score)
    {
        _state.highScore = score;
        Save();
    }

    public void SetCoins(int coins)
    {
        _state.coins = coins;
        Save();
    }

    [CanBeNull]
    public PlayerItem GetItem(string id)
    {
        return items.Find(item => item.id == id);
    }

    public List<PlayerItem> GetPlayerWearItems()
    {
        var list = new List<PlayerItem>();

        foreach (var itemId in _state.playerWears)
        {
            var item = GetItem(itemId);

            if (item != null)
            {
                list.Add(item);
            }
        }

        return list;
    }

    public bool HasItemInInventory(string itemId)
    {
        if(itemId == GameState.DefaultPlayerSkin)
        {
            return true;
        }

        return PlayerInventory.Exists(id => id == itemId);
    }

    public bool IsWearing(string itemId)
    {
        return PlayerWears.Exists(id => id == itemId);
    }

    public void Charge(int amount)
    {
        if (amount <= 0)
        {
            throw new Exception("Amount is less then zero");
        }

        var newCoins = Coins - amount;

        if (newCoins < 0)
        {
            throw new Exception("Trying to charge more then have");
        }

        SetCoins(newCoins);
    }

    public void ToggleWear(string itemId)
    {
        if (PlayerWears.Exists(id => itemId == id))
        {
            RemoveFromPlayerWears(itemId);
            return;
        }

        var items = GetPlayerWearItems();
        var newItem = GetItem(itemId);

        if (newItem == null)
        {
            return;
        }

        var exist = items.Find(item => item.category == newItem.category);

        if (exist != null)
        {
            RemoveFromPlayerWears(exist.id);
        }

        AddToPlayerWears(newItem.id);
    }

    public Skin GetPlayerSkinItem()
    {
        return GetSkin(PlayerSkinId);
    }

    public Sprite GetPlayerSkin()
    {
        return GetPlayerSkinItem().sprite;
    }

    [CanBeNull]
    public Skin GetSkin(string id)
    {
        return skins.Find(item => item.id == id);
    }

    public void SetPlayerSkin(string id)
    {
        _state.playerSkinId = id;
        playerSkinChanged.Invoke();
        Save();
    }

    public IEnumerator LoadLeaderboard(Leaderboard leaderboard)
    {
        CoroutineWithData cd = new CoroutineWithData(this, _service.GetLeaderboard());
        
        yield return cd.coroutine;

        if (cd.result == null)
        {
            leaderboard.Loaded(null);
            yield break;
        }
        
        leaderboard.Loaded((ApiService.LeaderboardPlayer[]) cd.result);
    }

    public IEnumerator Register(string nickname)
    {
        _state.nickname = nickname;
        CoroutineWithData cd = new CoroutineWithData(this, _service.Register(nickname));
        
        yield return cd.coroutine;

        if (cd.result == null)
        {
            yield break;
        }

        ApiService.RegisterRespond respond = (ApiService.RegisterRespond) cd.result;
        
        _state.lbgPlayerId = respond.playerId.ToString();
        _state.token = respond.token;
        Save(false);
    }

    public void ToggleMuteSound(Sound.SoundType type)
    {
        if (_state.mutedSounds.Exists(t => t == type))
        {
            _state.mutedSounds.Remove(type);
        }
        else
        {
            _state.mutedSounds.Add(type);
        }
        
        Save(false);
    }
    
    public bool IsMuted(Sound.SoundType type)
    {
        return MutedSounds.Exists(t => t == type);
    }

    public void UpdateNickname(string nickname)
    {
        _state.nickname = nickname;
        Save();
    }
    
    public bool IsLoggedIn()
    {
        return ! String.IsNullOrWhiteSpace(_state.token);
    }
    
}

[Serializable]
class GameState
{
    public static string DefaultPlayerSkin = "skin_white";

    public List<string> playerInventory = new List<string>();
    
    public List<string> playerWears = new List<string>();

    public string playerSkinId = DefaultPlayerSkin;

    public int highScore;

    public int coins = 50;

    public string nickname = "";
    
    public string lbgPlayerId = "";
    
    public string token = "";

    public List<Sound.SoundType> mutedSounds = new List<Sound.SoundType>();
}