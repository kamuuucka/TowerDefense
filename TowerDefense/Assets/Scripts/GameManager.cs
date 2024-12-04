using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private int lives;

    private void OnEnable()
    {
        //Subscribes the AssignPathEndPoint method to an event called "OnEnemySpawned".
        //This means whenever an enemy is spawned and this event is raised, the AssignPathEndPoint method will
        //be called with the spawned enemy as an argument.
        EventBus.Subscribe<Enemy>("OnEnemySpawned", AssignPathEndPoint);
        EventBus.Subscribe("OnEnemyReachedEnd", LoseLife);
    }

    private void OnDisable()
    {
        //Unsubscribes the AssignPathEndPoint method from the event called "OnEnemySpawned".
        //Prevents memory leaks.
        EventBus.Unsubscribe<Enemy>("OnEnemySpawned", AssignPathEndPoint);
        EventBus.Unsubscribe("OnEnemyReachedEnd", LoseLife);
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
        if (lives > 0)
        {
            lives--;
        }

        if (lives <= 0)
        {
            lives = 0;
            Debug.Log("Game over");
        }
    }
}
