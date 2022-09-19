using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("MapView");
    }

    public void toHowToMenu()
    {
        SceneManager.LoadScene("HowToMenuScene");
    }

    public void toAboutMenu()
    {
        SceneManager.LoadScene("AboutMenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
