using System.Collections;
using UnityEngine;

public class AreaOfDamageTower : BaseTower
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
        base.Attack(enemy);
        enemy.DamageEnemy(Damage); // Damage all enemies in range
    }
    
    private void OnEnemyDeath(Enemy enemy)
    {
        EnemiesInRange.Remove(enemy);
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (_aodVisual == null)
            {
                _aodVisual = TriggerAoDAttack(transform.position);
                Debug.Log($"After assigning {_aodVisual}");
            }
            if (EnemiesInRange.Count > 0)
            {
                for (int i = EnemiesInRange.Count - 1; i >= 0; i--)
                {
                    var enemy = EnemiesInRange[i];
                    if (enemy != null && _aodVisual != null)
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