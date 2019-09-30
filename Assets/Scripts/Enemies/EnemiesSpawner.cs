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
        public bool skipAssist;
        public bool enemiesMoving;
    }
    
    public GameObject[] enemiesPrefabs = new GameObject[0];
    public Difficulty[] difficulties;
    
    public float minDistance = 1.5f; 
    public float minX = -2.2f; 
    public float maxX = 2.2f;
    
    private int _currentDifficultyIndex = 0;
    private int _score => ScoreManager.instance.score;

    private bool _lastSpawn = false;
    
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
        int spawnCount = 0;
        Difficulty difficulty = GetDifficulty();
        fruits.RemoveAt(0);
        
        foreach (GameObject fruit in fruits)
        {
            if (difficulty.maxEnemiesPerBg <= spawnCount)
            {
                break;
            }

            if (_lastSpawn)
            {
                _lastSpawn = false;
                continue;
            }
            
            var mine = Random.Range(0, difficulty.diceSize);
            var result = Random.Range(0, difficulty.diceSize);

            if (mine == result)
            {
                _lastSpawn = true;
                spawnCount++;
                SpanEnemy(fruit);
            }
        }
    }

    private void SpanEnemy(GameObject fruit)
    {
        Difficulty difficulty = GetDifficulty();
        
        GameObject enemy = Instantiate(GetRandomEnemyPrefab(), Vector3.zero, Quaternion.identity, transform);

        if (difficulty.skipAssist)
        {
            Vector3 fruitPosition = fruit.transform.position;
            Vector3 temp = fruitPosition;
            temp.x = temp.x >= 0 ? temp.x * -1f : Mathf.Abs(temp.x);

            //check if fair
            float distance = temp.x >= 0 ? temp.x - fruitPosition.x : fruitPosition.x - temp.x;
            
            if (distance < minDistance)
            {
                float missingDistance = minDistance - distance;

                if (temp.x > 0)
                {
                    temp.x = Random.Range(temp.x + missingDistance, maxX);
                }
                else
                {
                    temp.x = Random.Range(minX, temp.x - missingDistance);
                }
            }

            enemy.transform.position = temp;
        }
        else
        {
            enemy.transform.position = fruit.transform.position;
            Destroy(fruit);
        }

        if (enemy.transform.position.x > 0)
        {
            enemy.GetComponent<Enemy>().FaceLeft();
        }
    }
    
}
