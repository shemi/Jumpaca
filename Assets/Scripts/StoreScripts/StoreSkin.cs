using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSkin : StoreItem
{
    private Skin _skin;

    private void Update()
    {
        if (!_inInventory && GameStateManager.instance.HasItemInInventory(_skin.id))
        {
            _inInventory = true;
            checkIconAnimator.SetBool("Show", true);
        }

        if (GameStateManager.instance.PlayerSkinId == _skin.id)
        {
            ToggleShadowStatus(true);
        }
        else
        {
            ToggleShadowStatus(false);
        }

    }

    public void SetItem(Skin item)
    {
        _skin = item;
        priceText.text = _skin.price == 0 ? "Free" : _skin.price.ToString();
        nameText.text = _skin.storeName;
        image.sprite = _skin.sprite;
    }
    
    public new void OnClick()
    {
        if (!_inInventory)
        {
            Buy();
        }
        else
        {
            Wear();
        }
    }

    private void Buy()
    {
        if (! MarketManager.instance.CanBuySkin(_skin.id))
        {
            ShowError();
            MarketManager.instance.ShowNoCoinsMessage();
            return;
        }

        MarketManager.instance.PlayBuySFX();
        MarketManager.instance.BuySkin(_skin.id);
    }

    private void Wear()
    {
        MarketManager.instance.PlayClickSFX();
        MarketManager.instance.SetSkin(_skin.id);
    }
    
}
