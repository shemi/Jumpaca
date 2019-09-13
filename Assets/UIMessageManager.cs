using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class UIMessageManager : MonoBehaviour
{
    public enum Type { Error, Success };
    
    public Image messageBackground;
    public Text messageText;

    public Color errorTextColor;
    public Color successTextColor;
    
    public float showDuration = 3.0f;

    public Sprite successSprite;
    public Sprite errorSprite;
    
    private Animator _animator;
    private readonly List<Message> _messages = new List<Message>();

    [CanBeNull] private Message _currentMessage;
    
    private bool _isShowing = false;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void PushMessage(Type type, string body)
    {
        if (_messages.Count > 0 && _messages.Last().message == body)
        {
            return;
        } 
        
        if (_messages.Count <= 0 && _currentMessage != null && _currentMessage.message == body)
        {
            return;
        }
        
        _messages.Add(new Message {type = type, message = body});
        Next();
    }

    void Next()
    {
        if (_isShowing)
        {
            return;
        }

        if (_messages.Count <= 0)
        {
            gameObject.SetActive(false);
            _currentMessage = null;
            return;
        }
        
        gameObject.SetActive(true);
        _currentMessage = _messages.First();
        _messages.RemoveAt(0);
        DisplayCurrentMessage();
        _isShowing = true;
    }

    void DisplayCurrentMessage()
    {
        if (_currentMessage == null)
        {
            return;
        }
        
        switch(_currentMessage.type)
        {
            case Type.Error:
                messageBackground.sprite = errorSprite;
                messageText.color = errorTextColor;
                break;
            case Type.Success:
                messageBackground.sprite = successSprite;
                messageText.color = successTextColor;
                break;
        }

        messageText.text = _currentMessage.message;
        _animator.SetBool("Show", true);
        StartCoroutine(HideMessage());
    }
    
    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(showDuration);
        _animator.SetBool("Show", false);
    }
    
    public void OutAnimationDone()
    {
        _isShowing = false;
        Next();
    }
}

class Message
{
    public UIMessageManager.Type type;
    public string message;

}
