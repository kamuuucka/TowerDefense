using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible for spawning the enemies from a spawner.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;

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
        StartCoroutine(SpawnEnemiesCoroutine(data.enemyAmount, data.delayBetweenEnemies));
    }

    /// <summary>
    /// Spawns the specified number of enemies with a delay between the spawns.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemiesCoroutine(int amountOfEnemies, float delayBetweenEnemies)
    {
        if (enemyPrefab == null) yield return null;
        for (var i = 0; i < amountOfEnemies; i++)
        {
            var enemy = Instantiate(enemyPrefab, gameObject.transform);
            
            EventBus.Publish("OnEnemySpawned", enemy);
            yield return new WaitForSeconds(delayBetweenEnemies);
        }
    }
}
