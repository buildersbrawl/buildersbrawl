using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

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
    public Text LevelStartBtnText;

    //checks if players one and player two is selected
    public bool playerOneSelected;
    public bool playerTwoSelected;
    public bool bothPlayersReady;

    //variables for assigning controller
    public int maxPlayers = 2;
    private List<PlayerMap> playerMap;
    private int gamePlayerIdCounter = 0;
    public InputManager inputManagerInstance;
    public bool controllerSelected = false;
    public int playerCounter = 0;

    public static Rewired.Player GetRewiredPlayer(int gamePlayerId)
    {
        if (!Rewired.ReInput.isReady)
            return null;
        if (PS == null)
        {
            Debug.Log("Not initialized");
            return null;
        }

        for (int i = 0; i < PS.playerMap.Count; i++)
        {
            if (PS.playerMap[i].gamePlayerId == gamePlayerId)
                return ReInput.players.GetPlayer(PS.playerMap[i].rewiredPlayerId);
        }
        return null;
    }

    void Awake()
    {
        playerMap = new List<PlayerMap>();
    }

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
        B4_P2_select.text = "Waiting for Player 1";


        //Hides the text
        P1_selected.enabled = false;
        P2_selected.enabled = false;

        LevelStartBtn.interactable = false;
        LevelStartBtnText.text = "Waiting for Players...";
    }

    // Update is called once per frame
    void Update()
    {
       
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            
            //Debug.Log(i);
            if (ReInput.players.GetPlayer(i).GetButtonDown("Submit"))
            {
                playerCounter++;
                //Debug.Log(playerCounter);
                AssignNextPlayer(i);

                //change ui to reflect a controller being selected
                if (playerCounter == 1)
                    SelectPlayerOne();
                else if (playerCounter == 2)
                    SelectPlayerTwo();
            }

        }
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
            B4_P2_select.text = "Press A to be Player 2";
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
            LevelStartBtnText.text = "Selecting Level...";
            Debug.Log("Both players selected");
        }
    }

    //assign a player to a controller and change their joystick to the in game joystick
    public void AssignNextPlayer(int rewiredPlayerId)
    {
        if (playerCounter == 2)
        {
            rewiredPlayerId = 1;
        }
        else if (playerCounter == 1)
        {
            rewiredPlayerId = 0;
        }
        else
        {
            Debug.Log("Max players - Tom");
            return;
        }

        //controllerSelected = true;
        Debug.Log("rewiredPlayerId = " + rewiredPlayerId);

        if (playerMap.Count >= maxPlayers)
        {
            Debug.Log("Max players");
            return;
        }

        int gamePlayerId = GetNextPlayerId();

        //set rewired player to next game player slot
        playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

        //Players.AssignControllerNumber(rewiredPlayerId, gamePlayerId);

        Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

        Debug.Log("Added Rewired Player id " + rewiredPlayerId + " to game player " + gamePlayerId);

        Debug.Log("rewiredPlayerId" + rewiredPlayerId);

    }

    //increment the playercounter
    public int GetNextPlayerId()
    {
        return gamePlayerIdCounter++;
    }

    //class to map the rewired player id to game player id
    public class PlayerMap
    {
        public int rewiredPlayerId;
        public int gamePlayerId;

        public PlayerMap(int rewiredPlayerID, int gamePlayerID)
        {
            this.rewiredPlayerId = rewiredPlayerID;
            this.gamePlayerId = gamePlayerID;
        }
    }
}
