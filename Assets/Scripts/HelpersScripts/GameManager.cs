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
    public Text doubleBananaCountText;

    private bool _isGameOver = true;
    
    private int _doubleBananaCount = 1;

    public int score => ScoreManager.instance.score;
    
    public bool isGameOver => _isGameOver;

    private bool _newHighScorePlayed = false;
    
    [SerializeField] private ParticleSystem newHighScorePS;

    [SerializeField]
    private Background[] backgroundSprites = new Background[0];
    
    private int _currentBackgroundSpriteIndex = 0;

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

    public void ResetDoubleBananaCount()
    {
        _doubleBananaCount = 1;
    }
    
    public void AddDoubleBananaCount()
    {
        _doubleBananaCount++;
    }
    
    void Update()
    {
        var positionY = player.position.y;

        if (positionY > 0.0f && positionY > score && ! _isGameOver)
        {
            var newScore = ((int) positionY) - score;
            ScoreManager.instance.UpdateScore(score + (newScore * _doubleBananaCount));
        }
        
        doubleBananaCountText.text = _doubleBananaCount + "X";
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

    public Sprite GetBackgroundSprite()
    {
        Background background = backgroundSprites[_currentBackgroundSpriteIndex];

        if (! background.IsActive())
        {
            background.Reset();
            _currentBackgroundSpriteIndex++;

            if (backgroundSprites.Length <= _currentBackgroundSpriteIndex)
            {
                _currentBackgroundSpriteIndex = 0;
            }
            
            background = backgroundSprites[_currentBackgroundSpriteIndex];
        }

        return background.GetSprite();
    }
    
}
