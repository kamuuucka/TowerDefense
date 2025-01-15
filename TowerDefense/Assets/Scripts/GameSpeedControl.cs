using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeedControl : MonoBehaviour
{
    [SerializeField] private float maximumSpeed = 5f;
    
    private float _currentSpeed = 1f;
    
    public void AdjustGameSpeed(float value)
    {
        if (maximumSpeed <= 0) maximumSpeed = 0.1f;
        Time.timeScale = Mathf.Clamp(value, 0.1f, maximumSpeed);
        _currentSpeed = Time.timeScale;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = _currentSpeed;
    }
}
