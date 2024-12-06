using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTower : BaseTower
{
    private Coroutine _attackCoroutine;
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
                for (int i = EnemiesInRange.Count - 1; i >= 0; i--)
                {
                    var enemy = EnemiesInRange[i];
                    if (enemy != null)
                    {
                        Attack(enemy);
                    }
                    else
                    {
                        EnemiesInRange.RemoveAt(i); // Remove null references
                    }
                }
            }

            yield return new WaitForSeconds(AttackInterval); // Wait for the attack interval
        }
    }
}
