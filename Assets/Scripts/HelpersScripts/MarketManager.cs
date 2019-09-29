using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;

    public UIMessageManager messages;

    [TextArea]
    public string noCoinsMessage;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SoundManager.instance.PlayBackgroundMusic();
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 100, 50), "Clear inventory"))
        {
            GameStateManager.instance.SetPlayerInventory(new List<string>());
            GameStateManager.instance.SetPlayerWears(new List<string>());
        }
        
        if (GUI.Button(new Rect(10, 130, 100, 50), "Add 100 coins"))
        {
            GameStateManager.instance.SetCoins(GameStateManager.instance.Coins + 100);
        }
    }
#endif
    
    public void AddCoins(int amount)
    {
        GameStateManager.instance.SetCoins(GameStateManager.instance.Coins + amount);
    }
    
    public bool CanBuy(string itemId)
    {
        var state = GameStateManager.instance;
        var item = state.GetItem(itemId);
        
        return item != null && item.price <= state.Coins;
    }

    public bool CanBuySkin(string itemId)
    {
        var state = GameStateManager.instance;
        var item = state.GetSkin(itemId);

        return item != null && item.price <= state.Coins;
    }

    public void ShowNoCoinsMessage()
    {
        messages.PushMessage(UIMessageManager.Type.Error, noCoinsMessage);
    }

    public void PlayBuySFX()
    {
        SoundManager.instance.BuyFX();
    }
    
    public void PlayClickSFX()
    {
        SoundManager.instance.ClickFX();
    }
    
    public void PlayErrorSFX()
    {
        SoundManager.instance.ErrorFX();
    }
    
    public void Buy(string itemId)
    {
        var state = GameStateManager.instance;
        var item = state.GetItem(itemId);

        if (item == null)
        {
            return;
        }

        state.Charge(item.price);
        state.AddItemToPlayerInventory(item.id);
        state.ToggleWear(item.id);
    }

    public void BuySkin(string id)
    {
        var state = GameStateManager.instance;
        var item = state.GetSkin(id);

        if (item == null)
        {
            return;
        }

        state.Charge(item.price);
        state.AddItemToPlayerInventory(item.id);
        state.SetPlayerSkin(item.id);
    }
    
    public void ToggleWear(string itemId)
    {
        var state = GameStateManager.instance;
        var item = state.GetItem(itemId);

        if (item == null)
        {
            return;
        }
        
        state.ToggleWear(item.id);
    }
    
    public void SetSkin(string skinId)
    {
        var state = GameStateManager.instance;
        var item = state.GetSkin(skinId);

        if (item == null)
        {
            return;
        }

        state.SetPlayerSkin(item.id);
    }

}
