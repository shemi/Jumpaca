using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public Transform player;
    public Text scoreText;
    public Text comboCountText;

    private bool _isGameOver = true;
    
    public int score => ScoreManager.instance.score;
    
    public bool isGameOver => _isGameOver;

    private bool _newHighScorePlayed = false;
    [SerializeField] private ParticleSystem newHighScorePS;

    private int _comboCount = 1;
    private string _comboType;
    
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        newHighScorePS.Stop(true);

        SoundManager.instance.PlayBackgroundMusic();
        
        StartGame();
    }

    public void RestartGame()
    {
        SoundManager.instance.ClickFX();
        ScenesManager.instance.GoToGameplay();
    }

    public void LoadMainMenu()
    {
        SoundManager.instance.ClickFX();
        ScenesManager.instance.GoToMainMenu();
    }

    public int GetComboCount()
    {
        return _comboCount;
    }
    
    public void AddComboCount(string type)
    {
        if (type == _comboType)
        {
            _comboCount++;
            return;
        }

        _comboType = type;
        _comboCount = 1;
    }
    
    void Update()
    {
        var positionY = player.position.y;
        var newScore = ((int) positionY) - score;
        
        if (newScore > 0 && score + newScore > score && ! _isGameOver)
        {
            ScoreManager.instance.UpdateScore(score + (newScore * _comboCount));
        }
        
        comboCountText.text = _comboCount + "X";
        scoreText.text = score.ToString();

        if (score > ScoreManager.instance.highScore && ! _newHighScorePlayed && ScoreManager.instance.highScore > 0)
        {
            newHighScore();
        }
    }

    private void newHighScore()
    {
        _newHighScorePlayed = true;
        newHighScorePS.Play(true);
        SoundManager.instance.HighScoreSoundFX();
    }
    
    private void StartGame()
    {
        _isGameOver = false;
    }
    
    public void GameOver()
    {
        if (score > ScoreManager.instance.highScore)
        {
            ScoreManager.instance.SaveHighScore(score);
        }
        
        ScoreManager.instance.SaveCoinsFromScore();
        
        _isGameOver = true;
    }

}
