using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutMenu : MonoBehaviour
{
    public void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
