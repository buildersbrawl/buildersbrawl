using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Canvas MainMenu;
    public Canvas Controls;

    void Start()
    {
        //Start by only showing the main menu screen
        MainMenu.gameObject.SetActive(true);
        Controls.gameObject.SetActive(false);
    }

    public void HowToPlayBtn()
    {
        //Show the player control screen
        MainMenu.gameObject.SetActive(false);
        Controls.gameObject.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        //Go straight to the main menu screen from any screen
        MainMenu.gameObject.SetActive(true);
        Controls.gameObject.SetActive(false);
    }
}