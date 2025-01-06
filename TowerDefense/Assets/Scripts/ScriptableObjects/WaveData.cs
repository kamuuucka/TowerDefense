using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "ScriptableObjects/CreateWaveData", order = 2)]
public class WaveData : ScriptableObject
{
    [SerializeField] public int enemyAmount;
    [SerializeField] public int delayBetweenEnemies;
    
}
