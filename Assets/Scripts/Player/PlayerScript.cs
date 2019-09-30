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

    private bool _initialPush;

    private int _pushCount;

    private string _lastFruitType;
    private int _fruitsCount;

    //SuperJump
    public Animator superJumpAnimator;
    public int superJumpComboCount = 3;
    public float superJumpPushVelocity = 25.0f;
    private bool _superJumpActive = false;
    private bool _isinstanceNull;

    private string _currentBackgroundId = "0";

    private void Start()
    {
        _isinstanceNull = GameManager.instance == null;
    }

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

        if (_superJumpActive && myBody.velocity.y <= 0.5f)
        {
            StopSuperJump();
        }
    }

    void StopSuperJump()
    {
        _superJumpActive = false;
        superJumpAnimator.SetBool("Show", false);
        SoundManager.instance.FadeOut("superJump", 0.050f);
    }
    
    void Move()
    {
        if (_isinstanceNull || GameManager.instance.isGameOver)
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
            var localScale = transform.localScale;
            
            if (slider.value > 0 && localScale.x > 0)
            {
                transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
            }
            
            if(slider.value < 0 && localScale.x < 0)
            {
                transform.localScale = new Vector3(Math.Abs(localScale.x), localScale.y, localScale.z);
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
        
        if(target.CompareTag("BG"))
        {
            GameManager.instance.PlayNewWorld(target.name);
        }

        if (target.CompareTag("Fruit"))
        {
            Fruit fruit = target.GetComponent<Fruit>();
            
            if (! _superJumpActive || myBody.velocity.y <= fruit.pushVelocity)
            {
                myBody.velocity = new Vector2(myBody.velocity.x, fruit.pushVelocity);
                GameManager.instance.AddComboCount(fruit.type);

                if (_superJumpActive)
                {
                    StopSuperJump();
                }
            }
            
            fruit.Hit();

            if (! _superJumpActive && GameManager.instance.GetComboCount() >= superJumpComboCount)
            {
                _superJumpActive = true;
                SoundManager.instance.Play("superJump");
                superJumpAnimator.SetBool("Show", true);
                GameManager.instance.AddComboCount("");
                myBody.velocity = new Vector2(myBody.velocity.x, superJumpPushVelocity);
            }
            
        }

        if (target.CompareTag("Enemy"))
        {
            Enemy enemy = target.GetComponent<Enemy>();

            if (_superJumpActive)
            {
                enemy.TakeHit();
                return;
            }
            
            enemy.Die();
            GameManager.instance.GameOver();
        }
        
        if (target.CompareTag("FallDown"))
        {
            GameManager.instance.GameOver();
        }
        
    }
}
