using System.Collections;
using UnityEngine;

public class AreaOfDamageTower : BaseTower
{
    private Coroutine _attackCoroutine;
    private GameObject _aodVisual;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe<Enemy>("EnemyDeath", OnEnemyDeath);
        _attackCoroutine = StartCoroutine(AttackLoop());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
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

    public override IEnumerator AttackLoop()
    {
        while (true)
        {
            if (_aodVisual == null)
            {
                _aodVisual = TriggerAoDAttack(transform.position);
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