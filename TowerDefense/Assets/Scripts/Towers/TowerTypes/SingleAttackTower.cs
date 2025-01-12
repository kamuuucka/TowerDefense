using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackTower : BaseTower
{
    private Coroutine _attackCoroutine;
    
    
    private void OnEnable()
    {
        EventBus.Subscribe<Enemy>("EnemyDeath", StopAttacking);
    }

    private void OnDisable()
    {
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
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackLoop(enemy));
        }
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

    private IEnumerator AttackLoop(Enemy enemy)
    {
        while (true)
        {
            //enemy.DamageEnemy(Damage); // Call the attack method
            base.CreateProjectile(enemy.transform);
            yield return new WaitForSeconds(AttackInterval); // Wait before the next attack
        }
    }
}
