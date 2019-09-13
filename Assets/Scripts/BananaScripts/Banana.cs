using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private Animator _effect;

    private void Awake()
    {
        _effect = gameObject.GetComponent<Animator>();
    }

    public void RemoveBanana()
    {
        _effect.SetBool("play", true);
    }

    public void Destroy()
    {
        Destroy(gameObject, 0.0f);
    }
    
}
