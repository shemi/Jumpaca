using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{

    public static PlatformSpawner instance;

    public int minPointsToBirds = 100;
    public int birdsDifficultyStep = 150;
    
    [SerializeField]
    private GameObject leftPlatform, rightPlatform;
    
    private float _leftXMin = -4.4f;
    private float _leftXMax = -2.8f;
    private float _rightXMin = 4.4f;
    private float _rightXMax = 2.8f;
    private float _yThreshold = 2.6f;
    private float _lastY;

    public int spawnCount = 8;

    private int platformSpawned;

    [SerializeField]
    private Transform platformParent;

    [SerializeField] private GameObject bird;
    public float birdY = 5f;
    private float _birdXMin = -2.3f;
    private float _birdXMax = 2.3f;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _lastY = transform.position.y;

        SpawnPlatforms();
    }

    public void SpawnPlatforms()
    {
        Vector2 temp = transform.position;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newPlatform = null;
            temp.y = _lastY;
            
            if ((platformSpawned % 2) == 0)
            {
                temp.x = UnityEngine.Random.Range(_leftXMin, _leftXMax);
                newPlatform = Instantiate(rightPlatform, temp, Quaternion.identity);
            }
            else
            {
                temp.x = UnityEngine.Random.Range(_rightXMin, _rightXMax);
                newPlatform = Instantiate(leftPlatform, temp, Quaternion.identity);
            }

            newPlatform.transform.parent = platformParent;
            _lastY += _yThreshold;
            platformSpawned++;
        }

        var score = GameManager.instance.score;

        if (score >= minPointsToBirds)
        {
            var difficulty = (int) Math.Floor(Math.Min(Math.Abs((float) score / birdsDifficultyStep), 2));
            difficulty = 7 - difficulty;
            
            var mine = Random.Range(0, difficulty);
            var result = Random.Range(0, difficulty);
            
            if (mine == result)
            {
                SpawnBird();
            }
        }
    }

    void SpawnBird()
    {
        Vector2 temp = transform.position;
        temp.x = Random.Range(_birdXMin, _birdXMax);
        temp.y += birdY;

        GameObject newBird = Instantiate(bird, temp, Quaternion.identity);
        newBird.transform.parent = platformParent;
    }
    
}
