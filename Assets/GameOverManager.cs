using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    public Animator animator;
    
    public SlidingNumber newCoinsText;

    [SerializeField] private GameObject gameOverPS;

    private bool _active = false;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver && ! _active)
        {
            _active = true;
            StartGameOver();
        }
    }

    void StartGameOver()
    {
        SoundManager.instance.StopBackgroundMusic();
        SoundManager.instance.GameOverSoundFX();
        gameOverPS.SetActive(true);
        animator.SetBool("Show", true);
    }

    public void GameOverAnimationEnded()
    {
        newCoinsText.SetNumber(ScoreManager.instance.GetCoinsFromScore());
    }

    public void CoinsCountEnded()
    {
        if (ScoreManager.instance.GetCoinsFromScore() < 50)
        {
            SoundManager.instance.BooSoundFX();
        }
        else
        {
            SoundManager.instance.WinSoundFX();
        }
        
        SoundManager.instance.PlayBackgroundMusic();
    }
    
}
