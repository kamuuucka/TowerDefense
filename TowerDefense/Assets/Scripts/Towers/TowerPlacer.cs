using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    //[SerializeField] private GameObject tower;
    [SerializeField] private List<BaseTower> towers;

    private int _usersMoney;
    private BaseTower _selectedTower;

    private void OnEnable()
    {
        EventBus.Subscribe<Transform>("TowerPlaced", SpawnTower);
        EventBus.Subscribe<int>("OnMoneyChanged", UpdateMoney);
        EventBus.Subscribe<GameObject>("TowerUpgraded", OnTowerUpgraded);
    }


    private void OnDisable()
    {
        EventBus.Unsubscribe<Transform>("TowerPlaced", SpawnTower);
        EventBus.Unsubscribe<int>("OnMoneyChanged", UpdateMoney);
        EventBus.Unsubscribe<GameObject>("TowerUpgraded", OnTowerUpgraded);
    }
    
    private void OnTowerUpgraded(GameObject selectedTower)
    {
        var meshFilter = selectedTower.GetComponentInChildren<MeshFilter>();
       // meshFilter.mesh = towerUpgraded;
    }

    private void Start()
    {
        UpdateMoney(GameManager.Instance.Money);
    }

    private void SpawnTower(Transform parent)
    {
        BaseTower newTower = Instantiate(_selectedTower, parent.position, Quaternion.identity);
        newTower.transform.SetParent(parent);
    }

    private void UpdateMoney(int money)
    {
        _usersMoney = money;
    }

    private void SelectTower(BaseTower.TowerType towerType)
    {
        foreach (var tower in towers)
        {
            Debug.Log(tower.towerType);
        }
        _selectedTower = towers.Find(tower => tower.towerType == towerType);

        if (_selectedTower != null)
        {
            Debug.Log(_selectedTower.name);
            Debug.Log(_selectedTower.Cost);
            if (_usersMoney >= _selectedTower.Cost)
            {
                _usersMoney -= _selectedTower.Cost;
                EventBus.Publish("OnMoneyChanged", _usersMoney);
            }
            else
            {
                Debug.Log("Not enough money to buy this tower.");
            }
        }
        else
        {
            Debug.LogError("Tower not found: " + towerType);
        }
    }

    public void BuySingleAttackTower()
    {
        SelectTower(BaseTower.TowerType.SingleAttack);
    }

    public void BuyDebuffTower()
    {
        //TODO: Why doesn't it see the debuff tower????
        SelectTower(BaseTower.TowerType.DebuffTower);
    }

    public void BuyAreaOfDamageTower()
    {
        SelectTower(BaseTower.TowerType.AreaOfDamage);
    }

    
}
