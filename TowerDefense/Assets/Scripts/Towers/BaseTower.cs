using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseTower : MonoBehaviour, ITowers
{
    #region ExposedVariables

    [SerializeField] private int cost;
    [SerializeField] private float damage;
    [SerializeField] private bool isDebug;
    [SerializeField] private float attackInterval = 1;
    [SerializeField] private GameObject projectile;
    
    [Header("UPGRADING SYSTEM")]
    [SerializeField] private int upgradeCost;
    [SerializeField] private float damageUpgrade;
    [SerializeField] private float intervalUpgrade;
    [SerializeField] private Button upgradeBtn;
    
    #endregion

    #region Private Variables

    private BoxCollider _collider;

    #endregion

    #region Protected Variables
    
    protected bool IsAttacking { get; private set; }
    protected List<Enemy> EnemiesInRange { get; } = new ();
    protected float AttackInterval => attackInterval;
    
    #endregion

    #region Public Variables
    
    public int Cost => cost;
    public float Damage => damage;
    
    #endregion



    protected virtual void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    protected virtual void OnEnable()
    {
        EventBus.Subscribe<int>("OnMoneyChanged", OnMoneyChanged);
    }

    protected virtual void OnDisable()
    {
        EventBus.Unsubscribe<int>("OnMoneyChanged", OnMoneyChanged);
    }

    private void OnMoneyChanged(int value)
    {
        if (value >= upgradeCost && GameManager.Instance.IsBuild)
        {
            upgradeBtn.gameObject.SetActive(true);
        }
        else
        {
            upgradeBtn.gameObject.SetActive(false);
        }
    }

    public virtual void Attack(Enemy enemy)
    {
       if (isDebug) Debug.Log($"{gameObject.name} is attacking!");
       IsAttacking = true;
    }

    public virtual void StopAttack(Enemy enemy)
    {
        if (isDebug) Debug.Log($"{gameObject.name} is stopping the attack!");
        IsAttacking = false;
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

    public virtual IEnumerator AttackLoop()
    {
        yield return null;
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
        EventBus.Publish("MoneyUpdate", -upgradeCost);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            EnemiesInRange.Add(enemy);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            EnemiesInRange.Remove(enemy);
        }
    }
    
   
}
