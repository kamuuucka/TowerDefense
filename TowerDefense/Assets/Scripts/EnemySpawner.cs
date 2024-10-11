using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int amountOfEnemies;
    [SerializeField] private float delayBetweenEnemies;

    public void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        for (var i = 0; i < amountOfEnemies; i++)
        {
            var enemy = Instantiate(enemyPrefab, gameObject.transform);
            
            EventBus.Publish("OnEnemySpawned", enemy);
            yield return new WaitForSeconds(delayBetweenEnemies);
        }
    }
}
