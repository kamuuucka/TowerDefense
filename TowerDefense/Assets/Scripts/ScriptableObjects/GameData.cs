using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "ScriptableObjects/CreateGameData", order = 1)]
public class GameData : ScriptableObject
{
    [HideInInspector] public int roundTimeMinutes;
    [HideInInspector] public int roundTimeSeconds;
    [HideInInspector] public int buildTimeMinutes;
    [HideInInspector] public int buildTimeSeconds;
    [SerializeField] private int _lives;
    [SerializeField] private int _money;
    [SerializeField] private List<WaveData> _waves;

    public int GetRoundTime()
    {
        return roundTimeMinutes * 60 + roundTimeSeconds;
    }

    public int GetBuildTime()
    {
        return buildTimeMinutes * 60 + buildTimeSeconds;
    }

    public int GetLives()
    {
        return _lives;
    }

    public int GetMoney()
    {
        return _money;
    }

    public List<WaveData> GetWaves()
    {
        return _waves;
    }
}
