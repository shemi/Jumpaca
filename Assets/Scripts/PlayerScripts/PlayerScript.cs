using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    [SerializeField]
    private Slider slider;
    
    private Rigidbody2D myBody;

    public bool disabled = false;
    
    public float moveSpeed = 10f;
    public float normalPush = 10f;
    public float extraPush = 14f;

    private bool _initialPush;

    private int _pushCount;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!disabled)
        {
            Move();
        }
    }

    void Move()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            myBody.velocity = new Vector2(moveSpeed, myBody.velocity.y);
        }
        
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            myBody.velocity = new Vector2(-moveSpeed, myBody.velocity.y);
        }

        if (Math.Abs(slider.value) > 0)
        {
            if (slider.value > 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
             
            if(slider.value < 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            
            myBody.velocity = new Vector2(slider.value * moveSpeed, myBody.velocity.y);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (disabled || GameManager.instance.isGameOver)
        {
            return;
        }
        
        if (target.CompareTag("ExtraPush"))
        {
            if (!_initialPush)
            {
                _initialPush = true;
                myBody.velocity = new Vector2(myBody.velocity.x, 18f);
                target.GetComponent<Banana>().RemoveBanana();
                
                SoundManager.instance.JumpSoundFX();
                
                return;
            }
        }

        if (target.CompareTag("NormalPush"))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, normalPush);
            target.GetComponent<Banana>().RemoveBanana();
            _pushCount++;
            
            GameManager.instance.ResetDoubleBananaCount();
            SoundManager.instance.JumpSoundFX();
        }
        
        if (target.CompareTag("ExtraPush"))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, extraPush);
            target.GetComponent<Banana>().RemoveBanana();
            _pushCount++;

            GameManager.instance.AddDoubleBananaCount();
            SoundManager.instance.JumpSoundFX();
        }

        if (_pushCount == 2)
        {
            _pushCount = 0;
            PlatformSpawner.instance.SpawnPlatforms();
        }

        if (target.CompareTag("FallDown") || target.CompareTag("Bird"))
        {
            GameManager.instance.GameOver();
        }
        
    }
}
