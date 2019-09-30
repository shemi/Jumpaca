using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstTimeController : MonoBehaviour
{

    public Animator animator;
    public TMP_InputField inputField;

    private bool _next = false;
    
    public void Show()
    {
        inputField.text = "Player" + Random.Range(0, 100);
        animator.SetBool("ShowNickname", true);
    }

    public void Close()
    {
        SoundManager.instance.Play("click");
        animator.SetBool("ShowNickname", false);
    }
    
    public void Next()
    {
        Close();
        SoundManager.instance.Play("click");
        StartCoroutine(GoToGameplay());
    }

    private IEnumerator GoToGameplay()
    {
        yield return new WaitForSeconds(0.40f);

        ScenesManager.instance.GoToGameplay();
    }
    
}
