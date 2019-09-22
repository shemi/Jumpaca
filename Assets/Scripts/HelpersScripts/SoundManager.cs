using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            CreateAudioSources();
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void CreateAudioSources()
    {
        foreach (Sound sound in sounds)
        {
            sound.setSource(gameObject.AddComponent<AudioSource>());
        }
    }

    public Sound GetSound(string soundName)
    {
        return Array.Find(sounds, sound => sound.name == soundName);
    }

    public void Play(string soundName)
    {
        GetSound(soundName).Play();
    }
    
    public void Stop(string soundName)
    {
        GetSound(soundName).Stop();
    }

    public void FadeIn(string soundName, float speed)
    {
        StartCoroutine(GetSound(soundName).FadeIn(speed));
    }

    public void FadeOut(string soundName, float speed)
    {
        StartCoroutine(GetSound(soundName).FadeOut(speed));
    }
    
    public void StopBackgroundMusic()
    {
        FadeOut("backgroundMusic", 0.015f);
    }
    
    public void PlayBackgroundMusic()
    {
        if (GetSound("backgroundMusic").isPlaying)
        {
            return;
        }
        
        FadeIn("backgroundMusic", 0.015f);
    }
    
    public void JumpSoundFX(float delay = 0f)
    {
        string jumpName = "jump";
        int jumpNumber = 1;

        if (GameManager.instance)
        {
            int count = GameManager.instance.GetComboCount();
            jumpNumber = count > 4 ? 4 : count;
        }
        
        Play(jumpName+jumpNumber);
    }
    
    public void GameOverSoundFX()
    {
        Play("gameOver");
    }
    
    public void ErrorFX()
    {
        Play("error");
    }
    
    public void ClickFX()
    {
        Play("click");
    }
    
    public void BuyFX()
    {
        Play("buy");
    }
    
    public void CoinFX(bool loop = false)
    {
        Play("coin");
    }
    
    public void StopCoinFX()
    {
        Stop("coin");
    }

    public void BooSoundFX()
    {
        Play("boo");
    }
    
    public void WinSoundFX()
    {
        Play("win");
    }
    
    public void HighScoreSoundFX()
    {
        Play("highScore");
    }

}
