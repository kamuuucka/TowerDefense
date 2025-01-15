using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Class responsible for the Enemy behaviour
/// </summary>
public class Enemy : MonoBehaviour
{
    #region Exposed Variables

    [SerializeField] private float speed = 1f;
    [SerializeField] private float health;
    [SerializeField] private int money;
    [SerializeField] private GameObject moneyUI;
    [SerializeField] private Slider slider;
    
    #endregion

    #region Private Variables

    private NavMeshAgent _agent;
    private Vector3 _target;
    private float _currentHealth;
    private bool _justSpawned = true;
    private bool _slowedDown;
    
    #endregion

    public int Money => money;

    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = speed;
        _currentHealth = health;
        slider.maxValue = health;
    }

    private void Update()
    {
        if (_justSpawned)
        {
            _agent.SetDestination(_target);
            _justSpawned = false;
        }
        slider.value = _currentHealth;

        if (!CheckIfFinished()) return;
        EventBus.Publish("OnEnemyReachedEnd");
        DestroyEnemy();
    }
    
    /// <summary>
    /// Sets the target for the gameObject.
    /// </summary>
    /// <param name="targetPoint">Point that will be the new object's target.</param>
    public void SetTarget(Vector3 targetPoint)
    {
        _target = targetPoint;
    }

    /// <summary>
    /// Deducts a specific amount from the _currentHealth.
    /// </summary>
    /// <param name="damage"> The amount that will be deducted.</param>
    public void DamageEnemy(float damage)
    {
        _currentHealth -= damage;
        if (!(_currentHealth <= 0) || transform == null) return;
        _currentHealth = 0;
        Instantiate(moneyUI, transform.position, Quaternion.identity);
        DestroyEnemy();
    }

    /// <summary>
    /// Slows down the enemy by the specific amount.
    /// </summary>
    /// <param name="slowDown">Amount that the enemy gets slowed down by.</param>
    public void SlowDownEnemy(float slowDown)
    {
        if (_slowedDown) return;
        _slowedDown = true;
        _agent.speed *= slowDown;
    }

    /// <summary>
    /// Restores the original speed of the enemy.
    /// </summary>
    public void RestoreOriginalSpeed()
    {
        if (!_slowedDown) return;
        _slowedDown = false;
        _agent.speed = speed;
    }

    /// <summary>
    /// Checks if the gameObject reached the target position.
    /// </summary>
    /// <returns>True if the gameObject reached the target, false if not.</returns>
    private bool CheckIfFinished()
    {
        return Math.Abs(transform.position.x - _target.x) < 0.01f && Math.Abs(transform.position.z - _target.z) < 0.01f;
    }

    /// <summary>
    /// Destroys the enemy object.
    /// </summary>
    private void DestroyEnemy()
    {
        EventBus.Publish("EnemyDeath", this);
        Destroy(gameObject);
    }
}
