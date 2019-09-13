using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreGridBuilder : MonoBehaviour
{

    public GameObject itemPrefab;

    void Start()
    {
        PopulateGrid();
    }

    void PopulateGrid()
    {

        foreach (var itemModel in GameStateManager.instance.items)
        {
            var item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, transform);
            item.GetComponent<StoreItem>().SetItem(itemModel);
        }
        
    }
    
}
