using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;
    
    public UnityEvent inAnimationDone;
    
    public UnityEvent outAnimationDone;

    public UnityEvent startOutAnimation;
    
    [CanBeNull] 
    private string _nextScene;

    public bool isReady = false;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            
            inAnimationDone = new UnityEvent();
            inAnimationDone = new UnityEvent();
            startOutAnimation = new UnityEvent();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        outAnimationDone.AddListener(GoToNext);
        inAnimationDone.AddListener(SetReady);
    }

    private void SetReady()
    {
        isReady = true;
    }
    
    private void GoToNext()
    {
        if (_nextScene == null)
        {
            return;
        }

        SceneManager.LoadScene(_nextScene);
        isReady = false;
        _nextScene = null;
    }

    public void GoToGameplay()
    {
        _nextScene = "Gameplay";
        playOutAnimation();
    }

    public void GoToMarket()
    {
        _nextScene = "Market";
        playOutAnimation();
    }
    public void GoToMainMenu()
    {
        _nextScene = "MainMenu";
        playOutAnimation();
    }

    public void playOutAnimation()
    {
        startOutAnimation.Invoke();
    }
    

    public void Back()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Gameplay":
            case "Market":
                GoToMainMenu();
                break;
        }
    }
    
}
