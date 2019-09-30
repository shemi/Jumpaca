using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("BG") || target.CompareTag("Fruit") || target.CompareTag("Enemy"))
        {
            Destroy(target.gameObject);
        }
        
    }
}
