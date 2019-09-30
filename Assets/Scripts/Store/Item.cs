using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{

   public abstract int GetPrice();

   public abstract Sprite GetStoreSprite();

   public abstract Sprite GetSprite();

    public abstract string GetStoreName();

    public abstract string GetID();

}
