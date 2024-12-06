using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private int lives;
    [SerializeField] private int money;

    private void OnEnable()
    {
        //Subscribes the AssignPathEndPoint method to an event called "OnEnemySpawned".
        //This means whenever an enemy is spawned and this event is raised, the AssignPathEndPoint method will
        //be called with the spawned enemy as an argument.
        EventBus.Subscribe<Enemy>("OnEnemySpawned", AssignPathEndPoint);
        EventBus.Subscribe("OnEnemyReachedEnd", LoseLife);
        EventBus.Subscribe<Enemy>("EnemyDeath", OnEnemyDeath);
        EventBus.Publish("OnLivesChanged", lives);
        EventBus.Publish("OnMoneyChanged", money);
    }
    
    private void OnDisable()
    {
        //Unsubscribes the AssignPathEndPoint method from the event called "OnEnemySpawned".
        //Prevents memory leaks.
        EventBus.Unsubscribe<Enemy>("OnEnemySpawned", AssignPathEndPoint);
        EventBus.Unsubscribe("OnEnemyReachedEnd", LoseLife);
        EventBus.Unsubscribe<Enemy>("EnemyDeath", OnEnemyDeath);
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
        switch (lives)
        {
            case > 0:
                lives--;
                break;
            case <= 0:
                lives = 0;
                break;
        }

        EventBus.Publish("OnLivesChanged", lives);
    }
    
    /// <summary>
    /// Add money when an enemy dies.
    /// </summary>
    /// <param name="enemy">Enemy that the money is taken from.</param>
    private void OnEnemyDeath(Enemy enemy)
    {
        money += enemy.Money;
        EventBus.Publish("OnMoneyChanged", money);
    }
}
