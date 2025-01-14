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
    [SerializeField] private GameObject projectile;
    
    [Header("UPGRADING SYSTEM")]
    [SerializeField] private int upgradeCost;
    [SerializeField] private float damageUpgrade;
    [SerializeField] private float intervalUpgrade;

    private BoxCollider _collider;
    private readonly List<Enemy> _enemiesInRange = new ();
    private bool _isAttacking;
    
    public enum TowerType
    {
        SingleAttack,
        AreaOfDamage,
        DebuffTower
    }
    
    public TowerType towerType;

    public int Cost => cost;
    public int UpgradeCost => upgradeCost;
    public float Damage => damage;

    public bool IsAttacking => _isAttacking;

    public List<Enemy> EnemiesInRange => _enemiesInRange;

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
        attackInterval -= intervalUpgrade;
    }

    public virtual void CreateProjectile(Transform target)
    {
        if (projectile != null && target != null)
        {
            GameObject spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            Projectile proj = spawnedProjectile.GetComponent<Projectile>();

            if (proj != null)
            {
                proj.Initialize(target, 5f, damage); // Example: speed = 10, use the tower's damage
            }
        }
    }
    
    public GameObject TriggerAoDAttack(Vector3 position)
    {
        if (projectile == null) return null;
        
        GameObject aodInstance = Instantiate(projectile, position, Quaternion.identity);
        AreaOfDamageVisual aodVisual = aodInstance.GetComponent<AreaOfDamageVisual>();
        if (aodVisual != null)
        {
            aodVisual.AssignDuration(attackInterval); // Use the attack interval as the duration
        }

        return aodInstance;
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
            _enemiesInRange.Add(enemy);
            //Attack(enemy);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            _enemiesInRange.Remove(enemy);
            //StopAttack(enemy);
        }
    }
    
   
}
