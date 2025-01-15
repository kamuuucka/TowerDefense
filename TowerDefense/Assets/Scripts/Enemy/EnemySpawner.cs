using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for spawning the enemies from a spawner.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    private void OnEnable()
    {
        EventBus.Subscribe<WaveData>("StartEnemySpawner", StartSpawningEnemies);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<WaveData>("StartEnemySpawner", StartSpawningEnemies);
    }

    /// <summary>
    /// Spawns the enemy with the help of coroutine.
    /// </summary>
    public void StartSpawningEnemies(WaveData data)
    {
        StartCoroutine(SpawnEnemiesCoroutine(data.enemies.Count, data.delayBetweenEnemies, data.enemies));
    }

    /// <summary>
    /// Spawns the specified number of enemies with a delay between the spawns.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemiesCoroutine(int amountOfEnemies, float delayBetweenEnemies, List<Enemy> enemies)
    {
        for (var i = 0; i < amountOfEnemies; i++)
        {
            if (enemies != null)
            {
                var enemy = Instantiate(enemies[i], gameObject.transform);
            
                EventBus.Publish("OnEnemySpawned", enemy);
            }

            yield return new WaitForSeconds(delayBetweenEnemies);
        }
    }
}
