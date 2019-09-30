using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitSpawner : MonoBehaviour
{
    [Serializable]
    public struct Difficulty
    {
        public int minScore;
        public int maxScore;
        public int diceSize;
        public int maxCombos;
    }
    
    public GameObject[] fruits;
    public Difficulty[] difficulties;
    public EnemiesSpawner enemiesSpawner;
    
    public float minX = -2.2f; 
    public float maxX = 2.2f; 
    
    public float minYThreshold = 2.6f;
    public float maxYThreshold = 3f;

    
    private int _spawnedCount = 0;
    
    //Combos helper
    public int maxFruitsPerCombo = 3;
    
    private GameObject _lastFruit;
    private int _currentDifficultyIndex = 0;
    [SerializeField]
    private int _comboCount = 0;
    private int _sameFruitCount = 0;
    private bool _creatingCombo = false;
    private int _score => ScoreManager.instance.score;

    private GameObject GetFruit()
    {
        Difficulty difficulty = GetDifficulty();

        if (_sameFruitCount + 1 >= maxFruitsPerCombo)
        {
            _creatingCombo = false;
            _sameFruitCount = 0;
            _lastFruit = GetRandomFruit();
            
            return _lastFruit;
        }
        
        if (_creatingCombo && _sameFruitCount + 1 < maxFruitsPerCombo)
        {
            _sameFruitCount++;
            
            return _lastFruit;
        }

        //lottery
        var mine = Random.Range(0, difficulty.diceSize);
        var result = Random.Range(0, difficulty.diceSize);

        _lastFruit = GetRandomFruit();

        if (mine == result && _comboCount < difficulty.maxCombos)
        {
            _creatingCombo = true;
            _sameFruitCount = 0;
            _comboCount++;
        }
        
        return _lastFruit;
    }

    private GameObject GetRandomFruit()
    {
        return fruits[Random.Range(0, fruits.Length)];
    }
    
    private Difficulty GetDifficulty()
    {
        Difficulty difficulty = difficulties[_currentDifficultyIndex];

        if (_score > difficulty.maxScore && _currentDifficultyIndex + 1 < difficulties.Length)
        {
            _currentDifficultyIndex++;
            _comboCount = 0;
            difficulty = difficulties[_currentDifficultyIndex];
        }

        return difficulty;
    }
    
    public void Spawn(float startY, float endY)
    {
        List<GameObject> fruitsToPass = new List<GameObject>();
        
        // Dont spawn fruit at the start of the bg
        float lastY = startY + 1f;
        endY -= 1f;
        
        while (true)
        {
            if (lastY > endY)
            {
                break;
            }

            float x;
            
            if (_creatingCombo)
            {
                x = Random.Range(-0.8f, 0.8f);
            }
            else
            {
                x = _spawnedCount % 2 == 0 ? Random.Range(0.8f, maxX) : Random.Range(minX, -0.8f);
            }

            Vector3 temp = new Vector3(x, lastY, 0f);
            GameObject fruitPrefab = GetFruit();
            
            GameObject fruit = Instantiate(fruitPrefab, Vector3.zero, Quaternion.identity);
            fruit.transform.parent = transform;
            fruit.transform.position = temp;
            lastY += Random.Range(minYThreshold, maxYThreshold);
            _spawnedCount++;
            fruitsToPass.Add(fruit);
        }

        enemiesSpawner.Spawn(fruitsToPass);
    }

}
