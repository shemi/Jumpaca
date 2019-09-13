using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    [SerializeField]
    private Boolean _tap = false;
    [SerializeField]
    private Boolean _isDragging = false;
    
    [SerializeField]
    private Vector2 _startTouch, _swipeDelta, _touchPosition;
    
    public Vector2 TouchPosition => _touchPosition;
    public Vector2 SwipeDelta => _swipeDelta;
    public Vector2 StartTouch => _startTouch;
    public Boolean Tap => _tap;
    public Boolean IsSwiping => _isDragging;

    private void Update()
    {
        _tap = false;
        
        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            _tap = true;
            _isDragging = true;
            _startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            Reset();
        }
        #endregion

        #region Mobile Inputs
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                _tap = true;
                _isDragging = true;
                _startTouch = Input.touches[0].position;
            } 
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                _isDragging = false;
                Reset();
            }
        }
        #endregion
        
        _swipeDelta = Vector2.zero;

        if (_isDragging)
        {
            if (Input.touches.Length > 0)
            {
                _touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                _swipeDelta = _touchPosition - _startTouch;
            } 
            else if (Input.GetMouseButton(0))
            {
                _touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _swipeDelta = _touchPosition - _startTouch;
            }
        }
        
    }

    private void Reset()
    {
        _startTouch = Vector2.zero;
        _swipeDelta = Vector2.zero;
        _isDragging = false;
    }
    
}
