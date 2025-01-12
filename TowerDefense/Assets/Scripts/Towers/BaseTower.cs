using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour, ITowers
{
    [SerializeField] private int cost;
    [SerializeField] private float damage;
    [SerializeField] private bool isDebug;
    [SerializeField] private float attackInterval = 1;
    //TODO: Hide this, no need to see it
    [SerializeField] private List<Enemy> enemiesInRange;
    
    [Header("UPGRADING SYSTEM")]
    [SerializeField] private int upgradeCost;
    [SerializeField] private float damageUpgrade;
    [SerializeField] private float intervalUpgrade;

    private BoxCollider _collider;
    private bool _isAttacking;

    public int Cost => cost;
    public int UpgradeCost => upgradeCost;
    public float Damage => damage;

    public bool IsAttacking => _isAttacking;

    public List<Enemy> EnemiesInRange => enemiesInRange;

    public float AttackInterval => attackInterval;

    protected virtual void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    public virtual void Attack(Enemy enemy)
    {
       if (isDebug) Debug.Log($"{gameObject.name} is attacking!");
       _isAttacking = true;
    }

    public virtual void StopAttack(Enemy enemy)
    {
        if (isDebug) Debug.Log($"{gameObject.name} is stopping the attack!");
        _isAttacking = false;
    }

    public virtual void Upgrade()
    {
        if (isDebug) Debug.Log($"{gameObject.name} is being upgraded!");
        damage += damageUpgrade;
        attackInterval += intervalUpgrade;
    }

    public void UpgradeTower()
    {
        Upgrade();
        EventBus.Publish("TowerUpgraded", gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
            //Attack(enemy);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
            //StopAttack(enemy);
        }
    }
    
   
}
