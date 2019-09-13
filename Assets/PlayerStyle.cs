using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerStyle : MonoBehaviour
{
    public GameObject headHolder, hairHolder, eyesHolder, faceHolder, neckHolder, legsHolder;

    public GameObject HeadHolder => headHolder;
    public GameObject HairHolder => hairHolder;
    public GameObject EyesHolder => eyesHolder;
    public GameObject FaceHolder => faceHolder;
    public GameObject NeckHolder => neckHolder;
    public GameObject LegsHolder => legsHolder;

    public object this[string propertyName] 
    {
        get{
            Type playerStyle = typeof(PlayerStyle);                   
            PropertyInfo myPropInfo = playerStyle.GetProperty(propertyName);

            if (myPropInfo == null)
            {
                throw new Exception("No property named: " + propertyName + " on PlayerStyle");
            }
            
            return myPropInfo.GetValue(this, null);
        }
    }
    
    void Start()
    {
        GameStateManager.instance.playerWearsChanged.AddListener(UpdatePlayerItems);

        if (GameStateManager.instance.loaded)
        {
            UpdatePlayerItems();
        }
    }

    private void UpdatePlayerItems()
    {
        List<PlayerItem> items = GameStateManager.instance.GetPlayerWearItems();
        var categories = Enum.GetValues(typeof(PlayerItem.Categories));
        
        foreach (var category in categories)
        {
            PlayerItem item = items.Find(i =>  category.Equals(i.category));
            GameObject itemHolder = (GameObject) this[category + "Holder"];
            
            if (item == null)
            {
                itemHolder.SetActive(false);
                continue;
            }

            itemHolder.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
            itemHolder.transform.localPosition = item.position;
            itemHolder.SetActive(true);
        }
        
        foreach (PlayerItem item in items)
        {
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    
}
