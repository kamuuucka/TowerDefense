using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text lives;
    [SerializeField] private TMP_Text money;
    [SerializeField] private TMP_Text time;
    private void OnEnable()
    {
        EventBus.Subscribe<int>("OnLivesChanged", UpdateLives);
        EventBus.Subscribe<int>("OnMoneyChanged", UpdateMoney);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<int>("OnLivesChanged", UpdateLives);
        EventBus.Unsubscribe<int>("OnMoneyChanged", UpdateMoney);
    }

    private void UpdateTime(float amount)
    {
        int minutes = Mathf.FloorToInt(amount / 60F);
        int seconds = Mathf.FloorToInt(amount - minutes * 60);

        string niceTime = $"{minutes:0}:{seconds:00}";

        time.text = niceTime;
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
