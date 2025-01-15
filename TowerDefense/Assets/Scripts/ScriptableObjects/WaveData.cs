using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Responsible for the wave data required by the GameData
/// </summary>
[CreateAssetMenu(fileName = "Wave Data", menuName = "ScriptableObjects/CreateWaveData", order = 2)]
public class WaveData : ScriptableObject
{
    [SerializeField] public float delayBetweenEnemies;
    [SerializeField] public List<Enemy> enemies;

}
