using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for the data of each game style
/// </summary>
[CreateAssetMenu(fileName = "Game Data", menuName = "ScriptableObjects/CreateGameData", order = 1)]
public class GameData : ScriptableObject
{
    #region Public Variables

    [HideInInspector] public int buildTimeMinutes;
    [HideInInspector] public int buildTimeSeconds;

    #endregion

    #region Private Variables

    [SerializeField] private int lives;
    [SerializeField] private int money;
    [SerializeField] private List<WaveData> waves;

    #endregion

    public int GetBuildTime()
    {
        return buildTimeMinutes * 60 + buildTimeSeconds;
    }

    public int GetLives()
    {
        return lives;
    }

    public int GetMoney()
    {
        return money;
    }

    public List<WaveData> GetWaves()
    {
        return waves;
    }
}
