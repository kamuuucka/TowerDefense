using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject tower;

    private void OnEnable()
    {
        EventBus.Subscribe<Transform>("TowerPlaced", SpawnTower);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<Transform>("TowerPlaced", SpawnTower);
    }

    private void SpawnTower(Transform parent)
    {
        Debug.Log("I am trying");
        GameObject newTower = Instantiate(tower, parent.position, Quaternion.identity);
        newTower.transform.SetParent(parent);
    }
}
