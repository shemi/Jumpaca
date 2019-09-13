using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerItem
{
    public enum Categories { Head, Hair, Eyes, Face, Neck, Legs };
    
    public string id;
    
    public string storeName;

    public Categories category;
    
    public Sprite iconSprite;
    public Sprite itemSprite;

    public int price;

    public Vector3 position;

}
