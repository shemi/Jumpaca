﻿using System;
using System.Linq;
using UnityEngine;

public class BgSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject background;

    private Transform[] _backgrounds;
    private float _height;
    private Vector3 _scale;
    private Vector3 _highestPosition;
    
    private Camera _mainCamera;
    
    void Awake()
    {
        _backgrounds = gameObject.GetComponentsInChildren<Transform>()
            .Where(go => go.gameObject != gameObject)
            .ToArray();
        
        _mainCamera = Camera.main;
        
        if (_mainCamera == null)
        {
            throw new Exception("No Camera!");
        }

    }

    private void Start()
    {
        _height = _backgrounds[0].gameObject.GetComponent<BoxCollider2D>().bounds.size.y;
        _highestPosition = _backgrounds[0].localPosition;
    }

    private void Update()
    {
        float threshold = _mainCamera.transform.position.y + (_height * 2);

        while (_highestPosition.y < Math.Abs(threshold))
        {
            Vector3 temp = _highestPosition;
            temp.y += _height;
            
            GameObject bg = Instantiate(background, Vector3.zero, Quaternion.identity, transform);
            bg.transform.localPosition = temp;

            _highestPosition = temp;
        }
    }
}