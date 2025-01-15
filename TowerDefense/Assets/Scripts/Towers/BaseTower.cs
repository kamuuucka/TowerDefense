using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Button deleteBtn;
    
    [Header("UPGRADING SYSTEM")]
    [SerializeField] private int upgradeCost;
    [SerializeField] private float damageUpgrade;
    [SerializeField] private float intervalUpgrade;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private float scaleJump = 0.05f;
    
    #endregion

    #region Private Variables

    private BoxCollider _collider;
    private Transform _towerVisual;
    private Vector3 _towerOriginalScale;

    #endregion

    #region Protected Variables
    
    protected bool IsAttacking { get; private set; }
    protected List<Enemy> EnemiesInRange { get; } = new ();
    protected float AttackInterval => attackInterval;
    
    #endregion

    #region Public Variables
    
    public int Cost => cost;
    public float Damage => damage;
    public GameObject Projectile => projectile;
    
    #endregion
    
    protected virtual void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    protected virtual void OnEnable()
    {
        EventBus.Subscribe<int>("OnMoneyChanged", SetUpgradeButton);
        
        foreach (Transform child in transform)
        {
            if (child.CompareTag("TowerVisual"))
            {
                _towerVisual = child;
                _towerOriginalScale = child.localScale;
                return;
            }
        }
    }

    protected virtual void OnDisable()
    {
        EventBus.Unsubscribe<int>("OnMoneyChanged", SetUpgradeButton);
    }

    /// <summary>
    /// Changes the visibility of the upgrade button depending on if the user can afford the upgrade.
    /// </summary>
    /// <param name="value">Money of the user.</param>
    private void SetUpgradeButton(int value)
    {
        if (_towerVisual != null)
        {
            if (GameManager.Instance.IsBuild && value >= upgradeCost && _towerVisual.localScale.x < _towerOriginalScale.x + scaleJump * 3)
            {
                upgradeBtn.gameObject.SetActive(true);
                deleteBtn.gameObject.SetActive(false);
            }
            else if (GameManager.Instance.IsBuild)
            {
                deleteBtn.gameObject.SetActive(true);
            }
            else
            {
                upgradeBtn.gameObject.SetActive(false);
                deleteBtn.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Attack the specific enemy.
    /// </summary>
    /// <param name="enemy">Enemy to attack.</param>
    public virtual void Attack(Enemy enemy)
    {
       if (isDebug) Debug.Log($"{gameObject.name} is attacking!");
       IsAttacking = true;
    }

    /// <summary>
    /// Stop attacking the specific enemy.
    /// </summary>
    /// <param name="enemy">Enemy that should not be attacked.</param>
    public virtual void StopAttack(Enemy enemy)
    {
        if (isDebug) Debug.Log($"{gameObject.name} is stopping the attack!");
        IsAttacking = false;
    }

    /// <summary>
    /// Upgrade the tower by adding the damage and subtracting the atttack interval speed.
    /// </summary>
    public virtual void Upgrade()
    {
        if (isDebug) Debug.Log($"{gameObject.name} is being upgraded!");
        damage += damageUpgrade;
        attackInterval -= intervalUpgrade;
    }

    /// <summary>
    /// Attack loop for each tower.
    /// </summary>
    public virtual IEnumerator AttackLoop()
    {
        yield return null;
    }

    /// <summary>
    /// Upgrade the tower and send the information to other scripts.
    /// </summary>
    public void UpgradeTower()
    {
        Upgrade();
        EventBus.Publish("TowerUpgraded", gameObject);
        EventBus.Publish("MoneyUpdate", -upgradeCost);
        
        if (_towerVisual != null)
        {
            _towerVisual.localScale += new Vector3(scaleJump, scaleJump, scaleJump);
        }
    }

    /// <summary>
    /// Add enemy to the list of enemies in range if they enter the collider.
    /// </summary>
    public virtual void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            EnemiesInRange.Add(enemy);
        }
    }

    /// <summary>
    /// Remove enemy from the list of enemies in range if they exit the collider.
    /// </summary>
    public virtual void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            EnemiesInRange.Remove(enemy);
        }
    }

    public void DeleteTower()
    {
        Destroy(gameObject);
    }
    
   
}
