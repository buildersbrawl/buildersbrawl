using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class Pause : MonoBehaviour
{   //2 buttons, resume & exit to main menu
    //public Button pause;
    //public Button exitToMenu;

    public GameObject pauseMenu;

    private void Start()
    {
        pauseMenu = this.gameObject;
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        //if (ReInput.players.GetPlayer(i).GetButtonDown("Submit")
    }

    //have tom run the pausegame function when a player presses start
    //change input from Default to UI in the same spot
    public void PauseGame()
    {
        InputManager.isUsingUI =true;
        //change time scale
        Time.timeScale = 0;

        //turn on pause menu
        pauseMenu.SetActive(true);

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);

        //change timescale back to 100%
        Time.timeScale = 1;
        InputManager.isUsingUI = false;
        //set the controller maps to default

    }

    public void ExitToMenu()
    {
        InputManager.isUsingUI = true;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
