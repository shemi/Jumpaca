using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static readonly int LeftAnimationId = Animator.StringToHash("Left");
    private static readonly int DieAnimationId = Animator.StringToHash("Die");
    private static readonly int HitAnimationId = Animator.StringToHash("Hit");
    
    public string type;
    
    private string _dieSoundName;
    private string _hitSoundName;

    private Animator _animator;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    [HideInInspector]
    public bool isDead = false;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();

        _dieSoundName = type + "Die";
        _hitSoundName = type + "Hit";
    }

    public void TakeHit()
    {
        isDead = true;
        _collider.isTrigger = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 0.5f;
        _animator.SetBool(HitAnimationId, true);
        SoundManager.instance.Play(_hitSoundName);
    }

    public void Die()
    {
        isDead = true;
        _animator.SetBool(DieAnimationId, true);
        SoundManager.instance.Play(_dieSoundName);
        
    }

    public void FaceLeft()
    {
        _animator.SetBool(LeftAnimationId, true);
    }
    
    public void FaceRight()
    {
        _animator.SetBool(LeftAnimationId, false);
    }
    
    void Update()
    {
        
    }
}
