using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToMenu : MonoBehaviour
{
    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void StartDisplay()
    {
        SceneManager.LoadScene("MapView");
    }

}
