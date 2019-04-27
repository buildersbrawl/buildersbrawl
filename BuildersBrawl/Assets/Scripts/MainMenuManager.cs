using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject Controls;
    public GameObject Options;
    public GameObject Credits;

    public Button StartGameBtn;
    public Button ControlsMM;
    public Button OptionsMM;
    public Button CreditsMM;

    public Button lastPressed;

    public Image BG;
    private float low = 0.25f;
    private float normal = 1f;

    void Start()
    {
        //Start by only showing the main menu screen
        MainScreen.SetActive(true);
        Controls.SetActive(false);
        Options.SetActive(false);
        Credits.SetActive(false);

        EventSystem.current.SetSelectedGameObject(StartGameBtn.gameObject, null);
        ChangeAlpha(normal);
    }

    public void StartGame(string PS_Scene)
    {
        //Load Player_Select scene
        SceneManager.LoadScene(PS_Scene);
    }

    public void HowToPlayBtn()
    {
        lastPressed = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        //Show the player control screen
        MainScreen.SetActive(false);
        Controls.SetActive(true);
        Options.SetActive(false);
        Credits.SetActive(false);

        EventSystem.current.SetSelectedGameObject(ControlsMM.gameObject, null);
        ChangeAlpha(low);
    }

    public void OptionsBtn()
    {
        lastPressed = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        MainScreen.SetActive(false);
        Controls.SetActive(false);
        Options.SetActive(true);
        Credits.SetActive(false);

        EventSystem.current.SetSelectedGameObject(OptionsMM.gameObject, null);
        ChangeAlpha(low);
    }

    public void CreditsBtn()
    {
        lastPressed = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        MainScreen.SetActive(false);
        Controls.SetActive(false);
        Options.SetActive(false);
        Credits.SetActive(true);

        EventSystem.current.SetSelectedGameObject(CreditsMM.gameObject, null);
        ChangeAlpha(low);
    }

    public void ReturnToMainMenu()
    {
        //Go straight to the main menu screen from any screen
        MainScreen.SetActive(true);
        Controls.SetActive(false);
        Options.SetActive(false);
        Credits.SetActive(false);

        EventSystem.current.SetSelectedGameObject(lastPressed.gameObject, null);
        ChangeAlpha(normal);
    }

    public void ChangeAlpha(float alphaValue)
    {
        var alphaColor = BG.color;
        alphaColor.a = alphaValue;
        BG.color = alphaColor;
    }
}