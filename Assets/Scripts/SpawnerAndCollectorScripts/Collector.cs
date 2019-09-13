using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("BG") || target.CompareTag("Platform") || 
            target.CompareTag("NormalPush") || target.CompareTag("ExtraPush") ||  
            target.CompareTag("Bird"))
        {
            Destroy(target.gameObject);
        }
        
    }
}
