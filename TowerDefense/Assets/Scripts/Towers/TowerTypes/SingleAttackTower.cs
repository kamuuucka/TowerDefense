using System.Collections;
using UnityEngine;

/// <summary>
/// Tower that attacks each enemy separately.
/// </summary>
public class SingleAttackTower : BaseTower
{
    private Coroutine _attackCoroutine;
    private Enemy _attackedEnemy;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe<Enemy>("EnemyDeath", StopAttacking);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe<Enemy>("EnemyDeath", StopAttacking);
    }

    private void Update()
    {
        if (!IsAttacking && EnemiesInRange.Count > 0)
        {
            Attack(EnemiesInRange[0]);
        }
    }

    public override void Attack(Enemy enemy)
    {
        base.Attack(enemy);
        StartAttacking(enemy);
    }
    
    /// <summary>
    /// Start the attack coroutine.
    /// </summary>
    /// <param name="enemy">Enemy that will be attacked.</param>
    private void StartAttacking(Enemy enemy)
    {
        if (_attackCoroutine != null || enemy == null) return;
        _attackedEnemy = enemy;
        _attackCoroutine = StartCoroutine(AttackLoop());
    }

    /// <summary>
    /// Stop the attack coroutine.
    /// </summary>
    /// <param name="enemy">Enemy that will stop being attacked.</param>
    private void StopAttacking(Enemy enemy)
    {
        if (!IsAttacking) return;
        EnemiesInRange.Remove(enemy);
        if (_attackCoroutine == null) return;
        StopCoroutine(_attackCoroutine);
        StopAttack(enemy);
        _attackCoroutine = null;

    }
    
    /// <summary>
    /// Create projectile and set up a target for it.
    /// </summary>
    /// <param name="target"></param>
    private void CreateProjectile(Transform target)
    {
        if (Projectile != null && target != null)
        {
            GameObject spawnedProjectile = Instantiate(Projectile, transform.position, Quaternion.identity);
            Projectile proj = spawnedProjectile.GetComponent<Projectile>();

            if (proj != null)
            {
                proj.Initialize(target, 5f, Damage); // Example: speed = 10, use the tower's damage
            }
        }
    }

    public override IEnumerator AttackLoop()
    {
        while (true)
        {
            CreateProjectile(_attackedEnemy.transform);
            yield return new WaitForSeconds(AttackInterval);
        }
    }
}
