using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject Controls;
    public GameObject Options;

    public Image BG;
    public float low = 0.6f;
    public float normal = 1f;

    void Start()
    {
        //Start by only showing the main menu screen
        MainScreen.SetActive(true);
        Controls.SetActive(false);
        Options.SetActive(false);

        ChangeAlpha(normal);
    }

    public void StartGame(string PS_Scene)
    {
        //Load Player_Select scene
        SceneManager.LoadScene(PS_Scene);
    }

    public void HowToPlayBtn()
    {
        //Show the player control screen
        MainScreen.SetActive(false);
        Controls.SetActive(true);
        Options.SetActive(false);

        ChangeAlpha(low);
    }

    public void OptionsBtn()
    {
        MainScreen.SetActive(false);
        Controls.SetActive(false);
        Options.SetActive(true);

        ChangeAlpha(low);
    }

    public void ReturnToMainMenu()
    {
        //Go straight to the main menu screen from any screen
        MainScreen.SetActive(true);
        Controls.SetActive(false);
        Options.SetActive(false);

        ChangeAlpha(normal);
    }

    public void ChangeAlpha(float alphaValue)
    {
        var alphaColor = BG.color;
        alphaColor.a = alphaValue;
        BG.color = alphaColor;
    }
}