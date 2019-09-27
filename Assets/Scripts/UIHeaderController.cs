using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIHeaderController : MonoBehaviour
{
    public Animator animator;
    
    [CanBeNull] public SlidingNumber coinsText;

    [CanBeNull] public SlidingNumber highScoreText;

    [CanBeNull] public SlidingNumber scoreText;
    
    private bool _isHighScoreTextNotNull;
    private bool _isCoinsTextNotNull;
    private bool _isScoreTextNotNull;

    private void Start()
    {
        _isScoreTextNotNull = scoreText != null;
        _isCoinsTextNotNull = coinsText != null;
        _isHighScoreTextNotNull = highScoreText != null;
        
        ScenesManager.instance.startOutAnimation.AddListener(OnStartOutAnimation);
    }

    void Update()
    {
        if (_isCoinsTextNotNull)
        {
            coinsText.SetNumber(GameStateManager.instance.Coins);
        }
        
        if (_isHighScoreTextNotNull)
        {
            highScoreText.SetNumber(GameStateManager.instance.HighScore);
        }

        if (_isScoreTextNotNull)
        {
            scoreText.SetNumber(ScoreManager.instance.score);
        }
    }

    public void Back()
    {
        SoundManager.instance.ClickFX();
        SoundManager.instance.StopCoinFX();
        ScenesManager.instance.Back();
    }

    public void OnInAnimationDone()
    {
        if (ScenesManager.instance)
        {
            ScenesManager.instance.inAnimationDone.Invoke();
        }
    }
    
    public void OnOutAnimationDone()
    {
        if (ScenesManager.instance)
        {
            ScenesManager.instance.outAnimationDone.Invoke();
        }
    }

    public void OnStartOutAnimation()
    {
        animator.SetBool("Out", true);
    }
    
}
