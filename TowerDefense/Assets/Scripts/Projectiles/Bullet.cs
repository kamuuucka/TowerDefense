using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float damage;

    public void Initialize(Transform target, float speed, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroy if target no longer exists
            return;
        }

        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Check if the projectile has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        // Apply damage to the enemy if it has a health component
        var enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.DamageEnemy(damage); // Assuming `Enemy` has a `TakeDamage` method
        }

        Destroy(gameObject); // Destroy the projectile
    }
}