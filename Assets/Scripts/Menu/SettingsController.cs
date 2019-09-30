using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using Toggle = UnityEngine.UI.Toggle;

public class SettingsController : MonoBehaviour
{
    public Toggle bgMusicToggle, sfxToggle, uiToggle;

    public TMP_InputField nicknameInput;

    public Animator animator;
    
    void Start()
    {
        if (GameStateManager.instance.loaded)
        {
            OnStateReady();
        }
        
        GameStateManager.instance.ready.AddListener(OnStateReady);
    }

    void OnStateReady()
    {
        bgMusicToggle.SetIsOnWithoutNotify(GameStateManager.instance.IsMuted(Sound.SoundType.MUSIC));
        sfxToggle.SetIsOnWithoutNotify(GameStateManager.instance.IsMuted(Sound.SoundType.SFX));
        uiToggle.SetIsOnWithoutNotify(GameStateManager.instance.IsMuted(Sound.SoundType.UI));
        nicknameInput.text = GameStateManager.instance.PlayerNickname;
    }
    
    public void OnUiMuteStageChange()
    {
        GameStateManager.instance.ToggleMuteSound(Sound.SoundType.UI);
        SoundManager.instance.Play("click");
    }
    
    public void OnSfxMuteStageChange()
    {
        GameStateManager.instance.ToggleMuteSound(Sound.SoundType.SFX);
        SoundManager.instance.Play("click");
    }
    
    public void OnBgMuteStageChange()
    {
        GameStateManager.instance.ToggleMuteSound(Sound.SoundType.MUSIC);
        
        if (GameStateManager.instance.IsMuted(Sound.SoundType.MUSIC))
        {
            SoundManager.instance.StopBackgroundMusic();
        }
        else
        {
            SoundManager.instance.PlayBackgroundMusic();
        }
        
        SoundManager.instance.Play("click");
    }

    public void UpdateNickname()
    {
        string nickname = nicknameInput.text;

        if (String.IsNullOrWhiteSpace(nickname))
        {
            return;
        }
        
        GameStateManager.instance.UpdateNickname(nickname.Trim());
    }

    public void Show()
    {
        SoundManager.instance.Play("click");
        animator.SetBool("ShowSettings", true);
    }

    public void Hide()
    {
        SoundManager.instance.Play("click");
        animator.SetBool("ShowSettings", false);
    }
    
}
