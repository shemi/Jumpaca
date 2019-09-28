﻿using System;
using System.Linq;
using UnityEngine;
 using UnityEngine.Serialization;

 public class BgSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject backgroundPrefab;

    [SerializeField]
    private FruitSpawner fruitSpawner;
    
    [SerializeField]
    private Background[] backgroundSprites = new Background[0];
    
    private float _highestYPosition = 0.5f;
    
    private Camera _mainCamera;

    private float _scale;
    private int _currentBackgroundSpriteIndex = 0;
    
    void Awake()
    {
        _mainCamera = Camera.main;
        
        if (_mainCamera == null)
        {
            throw new Exception("No Camera!");
        }

        ScaleToFitScreen();
    }

    private void ScaleToFitScreen()
    {
        float cameraHeight = _mainCamera.orthographicSize * 2.0f;
        float cameraWidth = cameraHeight * _mainCamera.aspect;

        Sprite sprite = GetBackgroundSprite();
        
        float unitWidth = sprite.textureRect.width / sprite.pixelsPerUnit;
        float unitHeight = sprite.textureRect.height / sprite.pixelsPerUnit;

        _scale = cameraWidth / unitWidth;
        
        gameObject.transform.localScale = new Vector3(_scale, _scale);
    }
    
    public Sprite GetBackgroundSprite()
    {
        Background background = backgroundSprites[_currentBackgroundSpriteIndex];

        if (! background.IsActive())
        {
            background.Reset();
            _currentBackgroundSpriteIndex++;

            if (backgroundSprites.Length <= _currentBackgroundSpriteIndex)
            {
                _currentBackgroundSpriteIndex = 0;
            }
            
            background = backgroundSprites[_currentBackgroundSpriteIndex];
        }

        return background.GetSprite();
    }

    private void Update()
    {
        float threshold = _mainCamera.transform.position.y + _scale;
        float highestYPositionRelativeToCamera = _highestYPosition * _scale;

        if (highestYPositionRelativeToCamera < Math.Abs(threshold))
        {
            Vector3 temp = new Vector3(0, _highestYPosition);
            temp.y += 1.0f;

            
            GameObject bg = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity, transform);
            bg.name = _currentBackgroundSpriteIndex.ToString();
            bg.transform.localPosition = temp;
            var spRenderer = bg.GetComponent<SpriteRenderer>();
            spRenderer.sprite = GetBackgroundSprite();
            var bgTransformPosition = bg.transform.position;
            var spRendererSize = spRenderer.bounds.size;

            Background background = backgroundSprites[_currentBackgroundSpriteIndex];
            if(background.IsStart())
            {
                bg.GetComponent<BoxCollider2D>().enabled = true;
            }

            fruitSpawner.Spawn(bgTransformPosition.y - (spRendererSize.y / 2), bgTransformPosition.y + (spRendererSize.y / 2));
            _highestYPosition = temp.y;
        }
    }
}