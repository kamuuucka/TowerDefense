using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowers
{
    public int Cost { get; }
    public int Damage { get; }
    public void Attack(Enemy enemy);
    public void Upgrade();
}
