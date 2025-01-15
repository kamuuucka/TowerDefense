using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Tower slowing down the enemies.
/// </summary>
public class DebuffTower : BaseTower
{
    private Coroutine _attackCoroutine;
    private GameObject _aodVisual;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe<Enemy>("EnemyDeath", RemoveEnemyFromRange);
        _attackCoroutine = StartCoroutine(AttackLoop());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe<Enemy>("EnemyDeath", RemoveEnemyFromRange);
        StopCoroutine(_attackCoroutine);
    }

    public override void Attack(Enemy enemy)
    {
        enemy.SlowDownEnemy(Damage);
    }

    public override void StopAttack(Enemy enemy)
    {
        base.StopAttack(enemy);
        enemy.RestoreOriginalSpeed();
    }

    /// <summary>
    /// Remove the enemy from the range of enemies.
    /// </summary>
    /// <param name="enemy">Enemy that will be removed.</param>
    private void RemoveEnemyFromRange(Enemy enemy)
    {
        EnemiesInRange.Remove(enemy);
    }
    
    /// <summary>
    /// Trigger the attack in the area.
    /// </summary>
    /// <param name="position">Position of the tower.</param>
    private void TriggerAoDAttack(Vector3 position)
    {
        if (Projectile == null) return;
        
        GameObject aodInstance = Instantiate(Projectile, position, Quaternion.identity);
        AreaOfDamageVisual aodVisual = aodInstance.GetComponent<AreaOfDamageVisual>();
        if (aodVisual != null)
        {
            aodVisual.AssignDuration(AttackInterval); // Use the attack interval as the duration
        }
    }

    public override IEnumerator AttackLoop()
    {
        while (true)
        {
            if (EnemiesInRange.Count > 0)
            {
                float attackDuration = AttackInterval * 0.8f;
                float cooldownDuration = AttackInterval * 0.2f;
                float elapsed = 0f;
                
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

    /// <summary>
    /// Process the actions for all the enemies in range. Used to for example stop the effect of the debuff.
    /// </summary>
    /// <param name="enemyAction">Action that will be performed.</param>
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
