using System;
using UnityEngine;

[Serializable]
public class Skin : Item
{
    public string id;

    public string storeName;  

    public Sprite sprite;

    public int price;

    public override string GetID()
    {
        return id;
    }

    public override int GetPrice()
    {
        return price;
    }

    public override Sprite GetSprite()
    {
        return sprite;
    }

    public override string GetStoreName()
    {
        return storeName;
    }

    public override Sprite GetStoreSprite()
    {
        return sprite;
    }
}
