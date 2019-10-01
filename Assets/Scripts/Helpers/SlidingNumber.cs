using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SlidingNumber : MonoBehaviour
{
    public Text numberText;
    public float animationTime = 50f;
    public string numberFormat = "0";
    public string prefix = "";
    public bool playCoinSound = false;

    public UnityEvent countEnded;
    
    private float _countSpeed = 0;
    private float _desiredNumber = 0;
    private float _initialNumber = 0;
    private float _currentNumber = 0;

    private bool _start;
    private bool _playingSFX = false;

    public void SetNumber(float value)
    {
        _initialNumber = _currentNumber;
        _desiredNumber = value;
        _start = true;

        if (_desiredNumber == _initialNumber)
        {
            numberText.text = prefix + _currentNumber.ToString(numberFormat);
        }

        else
        {
            _countSpeed = animationTime;

            if (_desiredNumber - _initialNumber > 500)
            {
                _countSpeed = animationTime * (_desiredNumber - _initialNumber) / 500;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_countSpeed == 0)
        {
            if(playCoinSound && _playingSFX)
            {
                SoundManager.instance.StopCoinFX();
            }

            if (_start)
            {
                countEnded.Invoke();
            }

            _start = false;
            return;
        }

        if (playCoinSound && !_playingSFX)
        {
            _playingSFX = true;
            SoundManager.instance.CoinFX(true);
        }

        if (_initialNumber < _desiredNumber)
        {
            _currentNumber += (_countSpeed * Time.deltaTime);

            if (_currentNumber > _desiredNumber)
            {
                _currentNumber = _desiredNumber;
                _countSpeed = 0;
            }
        }

        else
        {
            _currentNumber -= (_countSpeed * Time.deltaTime);

            if (_currentNumber <= _desiredNumber)
            {
                _currentNumber = _desiredNumber;
                _countSpeed = 0;

                if (_currentNumber < 0)
                {
                    _currentNumber = 0;
                }
                
            }
        }

        numberText.text = prefix + _currentNumber.ToString(numberFormat);
    }
}
