using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
[Serializable]
public class Sound
{
    public enum SoundType
    {
        MUSIC,
        SFX,
        UI
    }
    
    public string name;
    
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    
    [Range(.3f, 3f)]
    public float pitch = 1f;

    public bool loop = false;

    public bool playOnAwake = false;

    public SoundType type;
    
    [HideInInspector]
    public AudioSource source;

    public bool isPlaying => source.isPlaying;
    
    public void setSource(AudioSource s)
    {
        source = s;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;

        GameStateManager.instance.ready.AddListener(AfterReady);
    }

    void AfterReady()
    {
        if (playOnAwake)
        {
            Play();
        }
    }
    
    public void Play()
    {
        if (GameStateManager.instance.IsMuted(type) || isPlaying)
        {
            return;
        }
        
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public IEnumerator FadeOut(float speed)
    {
        if (! source.isPlaying)
        {
            yield break;
        }

        while (source.volume > 0f)
        {
            source.volume -= speed;
            
            yield return new WaitForSeconds(0.001f);
        }

        Stop();
        source.volume = volume;
    }
    
    public IEnumerator FadeIn(float speed)
    {
        source.volume = 0.000f;
        
        if (! source.isPlaying)
        {
            Play();
        }

        while (source.volume < volume)
        {
            source.volume += speed;
            
            yield return new WaitForSeconds(0.001f);
        }

        source.volume = volume;
    }
    
}
