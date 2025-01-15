using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for enabling specific panel at the end of the game. Also manages the buttons on the last screen.
/// </summary>
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
