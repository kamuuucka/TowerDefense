using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "ScriptableObjects/CreateWaveData", order = 2)]
public class WaveData : ScriptableObject
{
    [SerializeField] private int enemyAmount;
    [SerializeField] private int delayBetweenEnemies;
    
}
