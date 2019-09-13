using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    private PlayerItem _item;

    public Text priceText;
    public Text nameText;
    public Image image;
    public Shadow shadow;

    public Animator checkIconAnimator;
    public Animator errorIconAnimator;

    private bool _errorIsShown = false;
    private bool _inInventory = false;
    
    private void Update()
    {
        if (!_inInventory && GameStateManager.instance.HasItemInInventory(_item.id))
        {
            _inInventory = true;
            checkIconAnimator.SetBool("Show", true);
        }
        
        if (GameStateManager.instance.IsWearing(_item.id))
        {
            ToggleShadowStatus(true);
        }
        else
        {
            ToggleShadowStatus(false);
        }
    }

    void ToggleShadowStatus(bool active)
    {
        Color newColor = shadow.effectColor;
        newColor.a = active ? .23f : 0;
        shadow.effectColor = newColor;
    }

    public void SetItem(PlayerItem item)
    {
        _item = item;
        priceText.text = _item.price.ToString();
        nameText.text = _item.storeName;
        image.sprite = _item.iconSprite;
    }
    
    public void OnClick()
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

    private void ShowError()
    {
        if (_errorIsShown)
        {
            return;
        }
        
        MarketManager.instance.PlayErrorSFX();
        errorIconAnimator.SetBool("Show", true);
        _errorIsShown = true;
        StartCoroutine(HideError());
    }

    private IEnumerator HideError()
    {
        yield return new WaitForSeconds(2f);
        errorIconAnimator.SetBool("Show", false);
        _errorIsShown = false;
    }

    private void Buy()
    {
        if (! MarketManager.instance.CanBuy(_item.id))
        {
            ShowError();
            MarketManager.instance.ShowNoCoinsMessage();
            return;
        }

        MarketManager.instance.PlayBuySFX();
        MarketManager.instance.Buy(_item.id);
    }

    private void Wear()
    {
        MarketManager.instance.PlayClickSFX();
        MarketManager.instance.ToggleWear(_item.id);
    }
    
}
