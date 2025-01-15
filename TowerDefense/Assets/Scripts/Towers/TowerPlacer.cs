using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    //[SerializeField] private List<BaseTower> towers;

    private int _usersMoney;

    [HideInInspector] public BaseTower SelectedTower { get; private set; }

    private void OnEnable()
    {
        EventBus.Subscribe<Transform>("TowerPlaced", SpawnTower);
        EventBus.Subscribe<int>("OnMoneyChanged", UpdateMoney);
        EventBus.Subscribe<GameObject>("TowerUpgraded", OnTowerUpgraded);
        EventBus.Subscribe<BaseTower>("TowerSelected", SelectTower);
    }


    private void OnDisable()
    {
        EventBus.Unsubscribe<Transform>("TowerPlaced", SpawnTower);
        EventBus.Unsubscribe<int>("OnMoneyChanged", UpdateMoney);
        EventBus.Unsubscribe<GameObject>("TowerUpgraded", OnTowerUpgraded);
        EventBus.Unsubscribe<BaseTower>("TowerSelected", SelectTower);
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
        if (SelectedTower != null && parent != null)
        {
            BaseTower newTower = Instantiate(SelectedTower, parent.position, Quaternion.identity);
            newTower.transform.SetParent(parent);
            EventBus.Publish("MoneyUpdate", -SelectedTower.Cost);
            SelectedTower = null;
        }
        
    }

    private void UpdateMoney(int money)
    {
        _usersMoney = money;
    }

    private void SelectTower(BaseTower towerType)
    {
        SelectedTower = towerType;
        
        if (SelectedTower != null)
        {
            if (_usersMoney >= SelectedTower.Cost)
            {
                _usersMoney -= SelectedTower.Cost;
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
    

    
}
