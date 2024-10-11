using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform endPoint;

    private void OnEnable()
    {
        EventBus.Subscribe<Enemy>("OnEnemySpawned", AssignPathEndPoint);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<Enemy>("OnEnemySpawned", AssignPathEndPoint);
    }
     
    private void AssignPathEndPoint(Enemy enemy)
    {
        enemy.SetTarget(endPoint.position);
    }
}
