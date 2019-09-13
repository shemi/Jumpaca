﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
 public class Background
{
    public Sprite start, regular;
    public int maxAppearances;
    public int id;
    
    public int _appearances = 0;
    public int _loops = 0;
    
    public Sprite GetSprite()
    {
        _appearances += 1;
        
        if (_appearances == 1)
        {
            return start;
        }

        _appearances += 1;
        
        return regular;
    }

    public void Reset()
    {
        _appearances = 0;
        _loops++;
    }
    
    public bool IsActive()
    {
        return _appearances < maxAppearances;
    }
    
}
