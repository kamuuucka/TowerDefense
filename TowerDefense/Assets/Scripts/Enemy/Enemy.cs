using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float health;
    [SerializeField] private float money;
    [SerializeField] private GameObject moneyUI;
    [SerializeField] private Slider slider;
    private NavMeshAgent _agent;
    private Vector3 _target;
    private float _currentHealth;
    private bool _justSpawned = true;

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

        if (CheckIfFinished())
        {
            EventBus.Publish("OnEnemyReachedEnd");
            Instantiate(moneyUI, transform.position, Quaternion.identity);
            DestroyEnemy();
        }
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
        Debug.Log($"Enemy taking {damage} points of damage");
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Instantiate(moneyUI, transform.position, Quaternion.identity);
            DestroyEnemy();
        }
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
        Destroy(gameObject);
    }
}
