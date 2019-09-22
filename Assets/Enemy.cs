using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string type;
    
    private Animator _animator;
    private string _dieSoundName;
    private string _hitSoundName;
    
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _dieSoundName = type + "Die";
        _hitSoundName = type + "Hit";
    }

    public void TakeHit()
    {
        _animator.SetBool("Hit", true);
        SoundManager.instance.Play(_hitSoundName);
    }

    public void Die()
    {
        _animator.SetBool("Die", true);
        SoundManager.instance.Play(_dieSoundName);
        
    }
    
    void Update()
    {
        
    }
}
