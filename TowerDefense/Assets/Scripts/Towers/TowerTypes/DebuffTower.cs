using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTower : BaseTower
{
    private Coroutine _attackCoroutine;
    private GameObject _aodVisual;

    private void OnEnable()
    {
        EventBus.Subscribe<Enemy>("EnemyDeath", OnEnemyDeath);
        _attackCoroutine = StartCoroutine(AttackLoop());
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<Enemy>("EnemyDeath", OnEnemyDeath);
        StopCoroutine(_attackCoroutine);
    }

    public override void Attack(Enemy enemy)
    {
        enemy.SlowDownEnemy(Damage); // Damage all enemies in range
    }

    public override void StopAttack(Enemy enemy)
    {
        base.StopAttack(enemy);
        enemy.RestoreOriginalSpeed();
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        EnemiesInRange.Remove(enemy);
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (EnemiesInRange.Count > 0)
            {
                // Phase 1: Attack Duration
                float attackDuration = AttackInterval * 0.8f; // Attack for 80% of the interval
                float cooldownDuration = AttackInterval * 0.2f; // Cooldown for 20% of the interval
                float elapsed = 0f;

                // Trigger AoD attack once at the start of the attack duration
                TriggerAoDAttack(transform.position);
                Debug.Log("Triggered AoD attack at the start of the duration");

                while (elapsed < attackDuration)
                {
                    elapsed += Time.deltaTime;

                    ProcessEnemies(Attack);

                    yield return null; 
                }

                // Phase 2: Cooldown
                Debug.Log("Entering cooldown phase");
                ProcessEnemies(StopAttack);
                yield return new WaitForSeconds(cooldownDuration);
            }
            else
            {
                // No enemies in range, wait before checking again
                yield return new WaitForSeconds(AttackInterval);
            }
        }
    }

    private void ProcessEnemies(Action<Enemy> enemyAction)
    {
        for (int i = EnemiesInRange.Count - 1; i >= 0; i--)
        {
            var enemy = EnemiesInRange[i];
            if (enemy != null)
            {
                enemyAction(enemy); 
            }
            else
            {
                EnemiesInRange.RemoveAt(i);
            }
        }
    }


}
