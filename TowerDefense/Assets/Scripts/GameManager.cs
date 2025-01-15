using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages whole game.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Exposed Variables

    [SerializeField] private Transform endPoint;
    [SerializeField] private GameData data;
    [SerializeField] private UnityEvent onBuildOn;
    [SerializeField] private UnityEvent onBuildOff;
    
    #endregion
    
    #region Private Variables

    private int _lives;
    private int _money;
    private int _buildTime;
    private int _currentTime;
    private int _wave;
    private Coroutine _timerCoroutine;
    private int _enemiesToKill;
    private WaveData _activeWave;
    
    #endregion

    #region Public Variables
    
    public int Lives => _lives;
    public int Money => _money;
    public int Waves => _wave;
    [HideInInspector] public bool IsBuild;

    #endregion
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
        _activeWave = data.GetWaves()[_wave];
        _enemiesToKill = _activeWave.enemyAmount;
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
        EventBus.Subscribe<int>("MoneyUpdate", AdjustMoneyValue);
        IsBuild = true;
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
        EventBus.Unsubscribe<int>("MoneyUpdate", AdjustMoneyValue);
    }

    private void StartTimer(int time)
    {
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
            EventBus.Publish("OnTimeChanged", _currentTime);
            yield return new WaitForSeconds(1);
            _currentTime--;
        }
        EventBus.Publish("OnTimeChanged", 0);
        OnGameModeSwitched(false);

    }
    
    private void OnGameModeSwitched(bool isBuild)
    {
        IsBuild = isBuild;
        if (isBuild)
        {
            onBuildOn?.Invoke();
            EventBus.Publish("ModeSwitch", true);
            _wave++;
            if (_wave == data.GetWaves().Count)
            {
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                _activeWave = data.GetWaves()[_wave];
                _enemiesToKill = _activeWave.enemyAmount;
                EventBus.Publish("WavePassed", _wave);
                StartTimer(_buildTime);
                EventBus.Publish("OnMoneyChanged", _money);
            }
        }
        else
        {
            onBuildOff?.Invoke();
            EventBus.Publish("ModeSwitch", false);
            EventBus.Publish("StartEnemySpawner", _activeWave);
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
        switch (_lives)
        {
            case > 0:
                _lives--;
                break;
            case 0:
                SceneManager.LoadScene("GameOver");
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
        AdjustMoneyValue(enemy.Money);
        _enemiesToKill--;

        if (_enemiesToKill <= 0)
        {
            OnGameModeSwitched(true);
        }
    }
    
    private void AdjustMoneyValue(int cost)
    {
        _money += cost;
        EventBus.Publish("OnMoneyChanged", _money);
    }
}
