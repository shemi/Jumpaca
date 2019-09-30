using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    private int _score = 0;
    
    private int? _highScore = null;
    
    public int score => _score;
    public int highScore => GetHighScore();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void UpdateScore(int newScore)
    {
        _score = newScore;
    }

    public void SaveHighScore(int newHighScore)
    {
        _highScore = newHighScore;
        GameStateManager.instance.SetHighScore(newHighScore);
    }

    public void SaveCoinsFromScore()
    {
        GameStateManager.instance.SetCoins(GameStateManager.instance.Coins + GetCoinsFromScore());
    }
    
    public int GetCoinsFromScore()
    {
        return (int) Math.Floor(_score * 0.1);
    }
    
    public int GetHighScore()
    {
        return GameStateManager.instance.HighScore;
    }

    public void ResetHighScore()
    {
        _highScore = 0;
        
        GameStateManager.instance.SetHighScore(0);
    }
    
}
