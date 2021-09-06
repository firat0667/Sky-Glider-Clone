using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private UIGameOver uiControllerScript;

    private void Start()
    {
        Application.targetFrameRate = 300;
        uiControllerScript = this.GetComponent<UIGameOver>();
    }
    public void GameOver()
    {
        uiControllerScript.GameOver();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void EndGame()
    {
        uiControllerScript.FinishLevel();
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Ciktik");
    }
}
