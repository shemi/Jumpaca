using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BackgroundHelper : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        
        if (_mainCamera == null)
        {
            throw new Exception("No Camera!");
        }

        if (GameManager.instance)
        {
            Init();
        }
    }

    public void Init()
    {
        float cameraHeight = _mainCamera.orthographicSize * 2.0f;
        float cameraWidth = cameraHeight * _mainCamera.aspect;

        var sprite = GameManager.instance.GetBackgroundSprite();
        spriteRenderer.sprite = sprite;
        
        float unitWidth = sprite.textureRect.width / sprite.pixelsPerUnit;
        float unitHeight = sprite.textureRect.height / sprite.pixelsPerUnit;
        
        spriteRenderer.transform.localScale = new Vector3(cameraWidth / unitWidth, cameraWidth / unitWidth); //height / unitHeight
        boxCollider.size = sprite.bounds.size;
        boxCollider.offset = sprite.bounds.center;
        transform.localPosition = new Vector3(0, (boxCollider.bounds.size.y - cameraHeight) / 2, 0);
    }
}
