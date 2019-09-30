using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreGridBuilder : MonoBehaviour
{
    public GameObject itemPrefab;

    public bool isSkins = false;

    void Start()
    {
        PopulateGrid();
    }

    void PopulateGrid()
    {
        if(! isSkins)
        {
            foreach (var itemModel in GameStateManager.instance.items)
            {
                var item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, transform);
                item.GetComponent<StoreItem>().SetItem(itemModel);
            }
        }
        else
        {
            foreach (var itemModel in GameStateManager.instance.skins)
            {
                var item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, transform);
                item.GetComponent<StoreSkin>().SetItem(itemModel);
            }
        }
        
    }
    
}
