using UnityEngine;

/// <summary>
/// Manages placing specific towers on the grid.
/// </summary>
public class TowerPlacer : MonoBehaviour
{
    private int _usersMoney;
    private BaseTower _selectedTower;

    private void OnEnable()
    {
        EventBus.Subscribe<Transform>("TowerPlaced", SpawnTower);
        EventBus.Subscribe<int>("OnMoneyChanged", UpdateMoney);
        EventBus.Subscribe<BaseTower>("TowerSelected", SelectTower);
    }


    private void OnDisable()
    {
        EventBus.Unsubscribe<Transform>("TowerPlaced", SpawnTower);
        EventBus.Unsubscribe<int>("OnMoneyChanged", UpdateMoney);
        EventBus.Unsubscribe<BaseTower>("TowerSelected", SelectTower);
    }

    private void Start()
    {
        UpdateMoney(GameManager.Instance.Money);
    }

    /// <summary>
    /// Spawn specific tower on a grid cell.
    /// </summary>
    /// <param name="parent">The grid cell where the tower will be placed.</param>
    private void SpawnTower(Transform parent)
    {
        if (_selectedTower != null && parent != null)
        {
            BaseTower newTower = Instantiate(_selectedTower, parent.position, Quaternion.identity);
            newTower.transform.SetParent(parent);
            EventBus.Publish("MoneyUpdate", -_selectedTower.Cost);
            _selectedTower = null;
        }
        
    }
    
    /// <summary>
    /// Update the money that the user has.
    /// </summary>
    /// <param name="money">New amount of money.</param>
    private void UpdateMoney(int money)
    {
        _usersMoney = money;
    }

    /// <summary>
    /// Select a specific tower if the user has enough money.
    /// </summary>
    /// <param name="towerType">Type of the tower.</param>
    private void SelectTower(BaseTower towerType)
    {
        _selectedTower = towerType;

        if (_selectedTower == null) return;
        if (_usersMoney >= _selectedTower.Cost)
        {
            _usersMoney -= _selectedTower.Cost;
        }
    }
    

    
}
