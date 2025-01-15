using System.Collections;
using UnityEngine;

/// <summary>
/// Tower attacking all the enemies in range at once.
/// </summary>
public class AreaOfDamageTower : BaseTower
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
        base.Attack(enemy);
        enemy.DamageEnemy(Damage); 
    }
    
    /// <summary>
    /// Remove enemy from the range of the enemies.
    /// </summary>
    /// <param name="enemy"></param>
    private void RemoveEnemyFromRange(Enemy enemy)
    {
        EnemiesInRange.Remove(enemy);
    }
    
    /// <summary>
    /// Trigger the attack in the area.
    /// </summary>
    /// <param name="position">Position of the tower.</param>
    private GameObject TriggerAoDAttack(Vector3 position)
    {
        if (Projectile == null) return null;
        
        GameObject aodInstance = Instantiate(Projectile, position, Quaternion.identity);
        AreaOfDamageVisual aodVisual = aodInstance.GetComponent<AreaOfDamageVisual>();
        if (aodVisual != null)
        {
            aodVisual.AssignDuration(AttackInterval);
        }

        return aodInstance;
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