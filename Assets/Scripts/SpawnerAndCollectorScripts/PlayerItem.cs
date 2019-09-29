using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerItem : Item
{
    public enum Categories { Head, Hair, Eyes, Face, Neck, Legs };
    
    public string id;
    
    public string storeName;

    public Categories category;
    
    public Sprite iconSprite;
    public Sprite itemSprite;

    public int price;

    public Vector3 position;

    public override int GetPrice()
    {
        return price;
    }

    public override Sprite GetStoreSprite()
    {
        return iconSprite;
    }

    public override Sprite GetSprite()
    {
        return itemSprite;
    }

    public override string GetStoreName()
    {
        return storeName;
    }

    public override string GetID()
    {
        return id;
    }
}
