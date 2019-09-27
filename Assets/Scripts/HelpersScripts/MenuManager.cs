using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public Animator logoAnimator;

    public Leaderboard leaderboard;
    public FirstTimeController firstTimeController;
    
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

        if (GameStateManager.instance.IsLoggedIn())
        {
            ScenesManager.instance.GoToGameplay();
        }
        else
        {
            firstTimeController.Show();
        }
    }
    
    public void ShowLeaderboard()
    {
        SoundManager.instance.ClickFX();
        leaderboard.Show();
    }
    
    public void HideLeaderboard()
    {
        SoundManager.instance.ClickFX();
        leaderboard.Hide();
    }
    
    public void GoToStore()
    {
        SoundManager.instance.ClickFX();
        ScenesManager.instance.GoToMarket();
    }

}
