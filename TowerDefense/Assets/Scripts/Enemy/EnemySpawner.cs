using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int amountOfEnemies;
    [SerializeField] private float delayBetweenEnemies;

    private void OnEnable()
    {
        EventBus.Subscribe("StartEnemySpawner", SpawnEnemy);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("StartEnemySpawner", SpawnEnemy);
    }
    
    /// <summary>
    /// Spawns the enemy with the help of coroutine.
    /// </summary>
    public void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    /// <summary>
    /// Spawns the specified number of enemies with a delay between the spawns.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemiesCoroutine()
    {
        for (var i = 0; i < amountOfEnemies; i++)
        {
            var enemy = Instantiate(enemyPrefab, gameObject.transform);
            
            //Publish the "OnEnemySpawned" when the enemy is spawned.
            EventBus.Publish("OnEnemySpawned", enemy);
            yield return new WaitForSeconds(delayBetweenEnemies);
        }
    }
}
