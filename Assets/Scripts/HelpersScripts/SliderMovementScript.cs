using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMovementScript : MonoBehaviour
{

    [SerializeField]
    private Slider slider;

    private bool _isDragging = false;

    public float moveSpeed = 0.500f;

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
