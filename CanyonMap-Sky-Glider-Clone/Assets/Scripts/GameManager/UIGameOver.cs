using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    
    public GameObject GameOverButton;
    public GameObject FinishlaneButton;

    public Text fpsText;

    private void Update()
    {
        Fps();
    }
    public void GameOver()
    {
       GameOverButton.SetActive(true);
    }
    private void Fps()
    {
        fpsText.text = ( 1 / Time.smoothDeltaTime).ToString();
    }
    public void FinishLevel()
    {
        FinishlaneButton.SetActive(true);
    }
    
}
