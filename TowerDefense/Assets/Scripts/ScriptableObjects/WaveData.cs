using UnityEngine;

/// <summary>
/// Responsible for the wave data required by the GameData
/// </summary>
[CreateAssetMenu(fileName = "Wave Data", menuName = "ScriptableObjects/CreateWaveData", order = 2)]
public class WaveData : ScriptableObject
{
    [SerializeField] public int enemyAmount;
    [SerializeField] public int delayBetweenEnemies;
    
}
