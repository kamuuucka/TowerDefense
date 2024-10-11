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

    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = speed;
        _currentHealth = health;
        slider.maxValue = health;
    }

    private void Update()
    {
        _agent.SetDestination(_target);
        slider.value = _currentHealth;

        if (CheckIfFinished())
        {
            Instantiate(moneyUI, transform.position, Quaternion.identity);
            DestroyEnemy();
        }
    }
    
    public void SetTarget(Vector3 targetPoint)
    {
        _target = targetPoint;
    }

    public void DamageEnemy(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Instantiate(moneyUI, transform.position, Quaternion.identity);
            DestroyEnemy();
        }
    }

    private bool CheckIfFinished()
    {
        return Math.Abs(transform.position.x - _target.x) < 0.01f && Math.Abs(transform.position.z - _target.z) < 0.01f;
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
