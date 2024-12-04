using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackTower : BaseTower
{
    public override void Attack(Enemy enemy)
    {
        Debug.Log("Single Attack Tower is attacking!");
        enemy.DamageEnemy(Damage);
    }

    public override void Upgrade()
    {
        Debug.Log("Single Attack Tower is being upgraded!");
    }
}
