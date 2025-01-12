using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject tower;
    [SerializeField] private Mesh towerUpgraded;

    private void OnEnable()
    {
        EventBus.Subscribe<Transform>("TowerPlaced", SpawnTower);
        EventBus.Subscribe<GameObject>("TowerUpgraded", OnTowerUpgraded);
    }


    private void OnDisable()
    {
        EventBus.Unsubscribe<Transform>("TowerPlaced", SpawnTower);
        EventBus.Unsubscribe<GameObject>("TowerUpgraded", OnTowerUpgraded);
    }
    
    private void OnTowerUpgraded(GameObject selectedTower)
    {
        var meshFilter = selectedTower.GetComponentInChildren<MeshFilter>();
        meshFilter.mesh = towerUpgraded;
    }

    private void SpawnTower(Transform parent)
    {
        GameObject newTower = Instantiate(tower, parent.position, Quaternion.identity);
        newTower.transform.SetParent(parent);
    }
}
