using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverControl : MonoBehaviour
{
   [SerializeField] private GameObject gameOverPanel;
   [SerializeField] private GameObject gameWonPanel;

   private void Start()
   {
      if (GameManager.Instance.Lives == 0)
      {
         gameOverPanel.SetActive(true);
      }
      else
      {
         gameWonPanel.SetActive(true);
      }
   }

   public void RestartGame()
   {
      SceneManager.LoadScene("MainScene");
   }

   public void QuitGame()
   {
      Application.Quit();
   }
}
