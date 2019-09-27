using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    
    private const string fileName = "data.prz";
    
    private string filePath => Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName;

    private GameState _state = new GameState();

    public bool loaded = false;
    
    public UnityEvent playerInventoryChanged;
    
    public UnityEvent playerWearsChanged;

    public List<PlayerItem> items = new List<PlayerItem>();

    public List<string> PlayerInventory => _state.playerInventory;

    public List<string> PlayerWears => _state.playerWears;

    public int HighScore => _state.highScore;
    
    public int Coins => _state.coins;
    
    public string PlayerId => _state.lpgPlayerId;
    
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
        
        if (File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            GameState state = (GameState) binaryFormatter.Deserialize(file);
            file.Close();

            _state = state;
        }
        
        loaded = true;
    }

    void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(filePath);
        
        binaryFormatter.Serialize(file, _state);
        file.Close();
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
        if (! _state.playerWears.Exists(id => id == itemId))
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

    public bool IsLoggedIn()
    {
        return ! String.IsNullOrWhiteSpace(_state.token);
    }
    
}

[Serializable]
class GameState
{
    public List<string> playerInventory = new List<string>();
    
    public List<string> playerWears = new List<string>();
    
    public int highScore;

    public int coins = 50;

    public string lpgPlayerId = "";
    
    public string token = "";
}
