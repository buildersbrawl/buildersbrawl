using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{

    public static PlayerSelect PS;

    //Text before the players are selected
    public Text B4_P1_select;
    public Text B4_P2_select;

    //Text after players are selected
    public Text P1_selected;
    public Text P2_selected;

    public Button LevelStartBtn;

    //checks if players one and player two is selected
    public bool playerOneSelected;
    public bool playerTwoSelected;
    public bool bothPlayersReady;

    // Start is called before the first frame update
    void Start()
    {
        PS = this;

        //Shows that none of the players are selected
        playerOneSelected = false;
        playerTwoSelected = false;
        bothPlayersReady = false;

        //Displays the text that shows up before the players are selected
        B4_P1_select.enabled = true;
        B4_P2_select.enabled = true;

        //Hides the text
        P1_selected.enabled = false;
        P2_selected.enabled = false;

        LevelStartBtn.interactable = false;
    }

    public void SelectPlayerOne()
    {
        //Selects player one if not previously done
        if(!playerOneSelected)
        {
            playerOneSelected = true;
            Debug.Log("Player One selected");
            CheckSelectedPlayers();
        }
    }

    public void SelectPlayerTwo()
    {
        //Selects player two if not previously done and if player one was already selected
        if (!playerTwoSelected && playerOneSelected)
        {
            playerTwoSelected = true;
            Debug.Log("Player Two selected");
            CheckSelectedPlayers();
        }
    }

    void CheckSelectedPlayers()
    {
        //Checks if players one and two have been selected
        if (playerOneSelected)
        {
            B4_P1_select.enabled = false;
            P1_selected.enabled = true;
        }

        if (playerTwoSelected)
        {
            B4_P2_select.enabled = false;
            P2_selected.enabled = true;
        }

        //If players one and two were selected, then the bool becomes true
        if (playerOneSelected && playerTwoSelected)
        {
            bothPlayersReady = true;
            Debug.Log("Both players selected");
        }
    }
}
