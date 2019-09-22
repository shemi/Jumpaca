using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesSpawner : MonoBehaviour
{
   [Serializable]
    public struct Difficulty
    {
        public int maxScore;
        public int diceSize;
        public int maxEnemiesPerBg;
        public bool enemiesMoving;
    }
    
    public GameObject[] enemiesPrefabs = new GameObject[0];
    public Difficulty[] difficulties;
    
    private int _currentDifficultyIndex = 0;
    private int _score => ScoreManager.instance.score;
    
    private Difficulty GetDifficulty()
    {
        Difficulty difficulty = difficulties[_currentDifficultyIndex];

        if (_score > difficulty.maxScore && _currentDifficultyIndex + 1 < difficulties.Length)
        {
            _currentDifficultyIndex++;
            difficulty = difficulties[_currentDifficultyIndex];
        }

        return difficulty;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        return enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)];
    }
    
    public void Spawn(List<GameObject> fruits)
    {
        bool lastSpawn = false;
        int spawnCount = 0;
        Difficulty difficulty = GetDifficulty();

        foreach (GameObject fruit in fruits)
        {
            if (difficulty.maxEnemiesPerBg <= spawnCount)
            {
                break;
            }

            if (lastSpawn)
            {
                lastSpawn = false;
                continue;
            }
            
            var mine = Random.Range(0, difficulty.diceSize);
            var result = Random.Range(0, difficulty.diceSize);

            if (mine == result)
            {
                lastSpawn = true;
                spawnCount++;
                SpanEnemy(fruit);
            }
        }
    }

    private void SpanEnemy(GameObject fruit)
    {
        GameObject enemy = Instantiate(GetRandomEnemyPrefab(), Vector3.zero, Quaternion.identity, transform);
        enemy.transform.position = fruit.transform.position;
        Destroy(fruit);
    }
    
}
