using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text lives;
    [SerializeField] private TMP_Text money;
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text waves;
    private void OnEnable()
    {
        EventBus.Subscribe<int>("OnLivesChanged", UpdateLives);
        EventBus.Subscribe<int>("OnMoneyChanged", UpdateMoney);
        EventBus.Subscribe<int>("OnTimeChanged", UpdateTime);
        EventBus.Subscribe<int>("WavePassed", UpdateWaves);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<int>("OnLivesChanged", UpdateLives);
        EventBus.Unsubscribe<int>("OnMoneyChanged", UpdateMoney);
        EventBus.Unsubscribe<int>("OnTimeChanged", UpdateTime);
        EventBus.Unsubscribe<int>("WavePassed", UpdateWaves);
    }

    private void Start()
    {
        UpdateWaves(GameManager.Instance.Waves);
        UpdateMoney(GameManager.Instance.Money);
        UpdateLives(GameManager.Instance.Lives);
    }

    private void UpdateWaves(int waveNumber)
    {
        waves.text = $"Wave: {waveNumber + 1}";
    }

    private void UpdateTime(int amount)
    {
        int minutes = amount / 60;
        int seconds = amount - minutes * 60;

        string niceTime = $"{minutes:0}:{seconds:00}";

        time.text = $"Time left: {niceTime}";
    }

    private void UpdateMoney(int amount)
    {
        money.text = $"Money: {amount}";
    }

    private void UpdateLives(int amount)
    {
        lives.text = $"Lives: {amount}";
    }
}
