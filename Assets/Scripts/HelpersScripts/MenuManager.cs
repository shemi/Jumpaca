using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public Animator logoAnimator;
    
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 100, 50), "Reset high score"))
        {
            GameStateManager.instance.SetHighScore(0);
        }
    }
#endif
    
    private void Start()
    {
        ScenesManager.instance.inAnimationDone.AddListener(OnGameReady);
        SoundManager.instance.PlayBackgroundMusic();
    }

    private void OnGameReady()
    {
        logoAnimator.SetBool("SceneReady", true);
    }
    
    public void StartGame()
    {
        SoundManager.instance.ClickFX();
        ScenesManager.instance.GoToGameplay();
    }
    
    public void ShowLeaderboard()
    {
        SoundManager.instance.ClickFX();
    }
    
    public void GoToStore()
    {
        SoundManager.instance.ClickFX();
        ScenesManager.instance.GoToMarket();
    }

}
