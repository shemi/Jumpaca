using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderMovementScript : MonoBehaviour
{

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private GameObject sliderHandler;

    private bool _isDragging = false;

    public float moveSpeed = 0.500f;
    
    private RectTransform _transform;
    private Camera _camera;

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
        _camera = Camera.main;
    }

    public void StartDragging()
    {
        _isDragging = true;
    }
    
    public void StopDragging()
    {
        _isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_isDragging)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
            
                Vector2 touchPoint = _camera.ScreenToWorldPoint(touch.position);
                var position = _transform.position;
                position = new Vector3(position.x, touchPoint.y, position.z);
                _transform.position = position;
            }
            
            return;
        }

        var value = slider.value;

        if (slider.value > 0)
        {
            value -= moveSpeed;
            
            if (value < 0)
            {
                value = 0;
            } 
            
            slider.value = value;
        }
        
        else if (slider.value < 0)
        {
            value += moveSpeed;
            
            if (value > 0)
            {
                value = 0;
            } 
            
            slider.value = value;
        }

    }

}
