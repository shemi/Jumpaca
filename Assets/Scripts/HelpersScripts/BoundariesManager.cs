using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundariesManager : MonoBehaviour
{
    public BoxCollider2D leftBound, rightBound;

    public float width = 0.12f;
    
    private void Awake()
    {
        var camera = Camera.main;

        if (camera == null)
        {
            throw new Exception("No Camera!");
        }
        
        float cameraHeight = camera.orthographicSize * 2.0f;
        float cameraWidth = cameraHeight * camera.aspect;
        
        
        leftBound.size = new Vector2(width, cameraHeight);
        leftBound.offset = new Vector2(-(cameraWidth / 2f), 0f);
        
        rightBound.size = new Vector2(width, cameraHeight);
        rightBound.offset = new Vector2(cameraWidth / 2f, 0f);
    }

}
