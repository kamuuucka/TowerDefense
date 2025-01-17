using UnityEngine;

/// <summary>
/// Responsible for the projectiles in the scene.
/// </summary>
public class Projectile : MonoBehaviour
{
    private Transform _target;
    private float _speed;
    private float _damage;


    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, _target.position) < 0.1f)
        {
            HitTarget();
        }
    }

    /// <summary>
    /// Damage the hit target and destroy itself.
    /// </summary>
    private void HitTarget()
    {
        var enemy = _target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.DamageEnemy(_damage);
        }

        Destroy(gameObject);
    }
    
    /// <summary>
    /// Initialise the specifics of the projectile.
    /// </summary>
    /// <param name="target">Target to follow.</param>
    /// <param name="speed">Speed with which it should move.</param>
    /// <param name="damage">Damage that it should deal.</param>
    public void Initialize(Transform target, float speed, float damage)
    {
        _target = target;
        _speed = speed;
        _damage = damage;
    }
}