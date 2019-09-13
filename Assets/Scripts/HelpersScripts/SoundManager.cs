using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [SerializeField] private AudioSource backgroundMusic;
    
    [SerializeField] private AudioSource soundFX, sounds;
    
    [SerializeField] private AudioClip gameOverClip, 
                                       buy,
                                       error,
                                       superJumpStart,
                                       superJumpBlase,
                                       click,
                                       coin,
                                       win,
                                       boo,
                                       highScore;

    [SerializeField] private AudioClip[] jumpClips;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }
    
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            return;
        }
        
        backgroundMusic.Play();
    }
    
    public void JumpSoundFX(float delay = 0f)
    {
        soundFX.clip = jumpClips[Random.Range(0, jumpClips.Length)];
        soundFX.PlayDelayed(delay);
    }
    
    public void GameOverSoundFX()
    {
        soundFX.clip = gameOverClip;
        soundFX.Play();
    }
    
    public void ErrorFX()
    {
        soundFX.clip = error;
        soundFX.Play();
    }
    
    public void ClickFX()
    {
        soundFX.clip = click;
        soundFX.Play();
    }
    
    public void BuyFX()
    {
        soundFX.clip = buy;
        soundFX.Play();
    }
    
    public void CoinFX(bool loop = false)
    {
        soundFX.clip = coin;
        soundFX.loop = loop;
        
        soundFX.Play();
    }
    
    public void StopCoinFX()
    {
        soundFX.loop = false;
        
        soundFX.Stop();
    }

    public void BooSoundFX()
    {
        sounds.clip = boo;
        sounds.Play();
    }
    
    public void WinSoundFX()
    {
        sounds.clip = win;
        sounds.Play();
    }
    
    public void HighScoreSoundFX()
    {
        sounds.clip = highScore;
        sounds.Play();
    }
    
}
