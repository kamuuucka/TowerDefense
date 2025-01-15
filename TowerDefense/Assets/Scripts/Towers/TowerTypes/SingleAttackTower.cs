using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private void StartAttacking(Enemy enemy)
    {
        if (_attackCoroutine != null || enemy == null) return;
        _attackedEnemy = enemy;
        _attackCoroutine = StartCoroutine(AttackLoop());
    }

    private void StopAttacking(Enemy enemy)
    {
        if (IsAttacking)
        {
            EnemiesInRange.Remove(enemy);
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                StopAttack(enemy);
                _attackCoroutine = null;
            }
        }
        
    }

    public override IEnumerator AttackLoop()
    {
        while (true)
        {
            base.CreateProjectile(_attackedEnemy.transform);
            yield return new WaitForSeconds(AttackInterval);
        }
    }
}
