using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowers
{
    public int Cost { get; }
    public float Damage { get; }
    public void Attack(Enemy enemy);
    public void StopAttack(Enemy enemy);
    public void Upgrade();
    public void CreateProjectile(Transform target);

    public IEnumerator AttackLoop();
}
