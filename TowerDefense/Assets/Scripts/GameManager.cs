using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private GameData data;
    [SerializeField] private UnityEvent onBuildOn;
    [SerializeField] private UnityEvent onBuildOff;

    private int _lives;
    private int _money;
    private int _buildTime;
    private int _roundTime;
    private int _currentTime;
    private int _wave;
    private bool _isBuild;
    private Coroutine _timerCoroutine;

    public int Lives => _lives;
    public int Money => _money;
    public int Waves => _wave;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        _lives = data.GetLives();
        _money = data.GetMoney();
        _buildTime = data.GetBuildTime();
        _roundTime = data.GetRoundTime();
        _wave = data.GetWaves();

        _isBuild = true;
    }

    private void OnEnable()
    {
        //Subscribes the AssignPathEndPoint method to an event called "OnEnemySpawned".
        //This means whenever an enemy is spawned and this event is raised, the AssignPathEndPoint method will
        //be called with the spawned enemy as an argument.
        EventBus.Subscribe<Enemy>("OnEnemySpawned", AssignPathEndPoint);
        EventBus.Subscribe("OnEnemyReachedEnd", LoseLife);
        EventBus.Subscribe<Enemy>("EnemyDeath", OnEnemyDeath);
        EventBus.Subscribe<bool>("GameModeSwitch", OnGameModeSwitched);

        StartTimer(_buildTime);
    }
    
    private void OnDisable()
    {
        //Unsubscribes the AssignPathEndPoint method from the event called "OnEnemySpawned".
        //Prevents memory leaks.
        EventBus.Unsubscribe<Enemy>("OnEnemySpawned", AssignPathEndPoint);
        EventBus.Unsubscribe("OnEnemyReachedEnd", LoseLife);
        EventBus.Unsubscribe<Enemy>("EnemyDeath", OnEnemyDeath);
        EventBus.Unsubscribe<bool>("GameModeSwitch", OnGameModeSwitched);
    }

    private void StartTimer(int time)
    {
        // Stop any running timer before starting a new one
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }

        _timerCoroutine = StartCoroutine(TimerRoutine(time));
    }

    private IEnumerator TimerRoutine(int time)
    {
        _currentTime = time;

        while (_currentTime > 0)
        {
            //Debug.Log($"Time Left: {_currentTime}");
            EventBus.Publish("OnTimeChanged", _currentTime);
            yield return new WaitForSeconds(1);
            _currentTime--;
        }

        _isBuild = !_isBuild;
        OnGameModeSwitched(_isBuild);

    }
    
    private void OnGameModeSwitched(bool isBuild)
    {
        if (isBuild)
        {
            onBuildOn?.Invoke();
            EventBus.Publish("ModeSwitch", true);
            _wave--;
            EventBus.Publish("WavePassed", _wave);
            StartTimer(_buildTime);
        }
        else
        {
            onBuildOff?.Invoke();
            EventBus.Publish("ModeSwitch", false);
            EventBus.Publish("StartEnemySpawner");
            StartTimer(_roundTime);
        }
    }

    /// <summary>
    /// Assigns the endPoint as the target of the enemy.
    /// </summary>
    /// <param name="enemy">The enemy that the target should be assigned to.</param>
    private void AssignPathEndPoint(Enemy enemy)
    {
        enemy.SetTarget(endPoint.position);
    }
    
    /// <summary>
    /// Remove one life.
    /// </summary>
    private void LoseLife()
    {
        switch (data.GetLives())
        {
            case > 0:
                _lives--;
                break;
            case <= 0:
                _lives = 0;
                break;
        }

        EventBus.Publish("OnLivesChanged", _lives);
    }
    
    /// <summary>
    /// Add money when an enemy dies.
    /// </summary>
    /// <param name="enemy">Enemy that the money is taken from.</param>
    private void OnEnemyDeath(Enemy enemy)
    {
        _money += enemy.Money;
        EventBus.Publish("OnMoneyChanged", _money);
    }
}
