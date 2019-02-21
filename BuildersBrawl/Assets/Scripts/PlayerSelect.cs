using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    public static PlayerSelect S;

    //players
    public int p1Controller;
    public int p2Controller;

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

    public LevelSelector ls;

    public bool inPlayerSelect = true;
    private bool initialized = false;

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
        //if another one of these get rid of this one
        if(PlayerSelect.S == null)
        {
            S = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        playerMap = new List<PlayerMap>();

        //InitializeScene();
    }

    // Start is called before the first frame update
    void Start()
    {
        PS = this;

        //don't destory when going to next scene
        DontDestroyOnLoad(this.gameObject);

        //Shows that none of the players are selected
        playerOneSelected = false;
        playerTwoSelected = false;
        bothPlayersReady = false;

        //Displays the text that shows up before the players are selected
        if(inPlayerSelect)
        {
            B4_P1_select.enabled = true;
            B4_P2_select.enabled = true;
            B4_P2_select.text = "Waiting for Player 1";


            //Hides the text
            P1_selected.enabled = false;
            P2_selected.enabled = false;

            LevelStartBtn.interactable = false;
            LevelStartBtnText.text = "Waiting for Players...";
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckLevel();
        if (inPlayerSelect)
        {
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                //Debug.Log(i);
                if (ReInput.players.GetPlayer(i).GetButtonDown("Submit"))
                {
                    playerCounter++;

                    //change ui to reflect a controller being selected
                    if (playerCounter == 1)
                        SelectPlayerOne();
                    else if (playerCounter == 2)
                        SelectPlayerTwo();

                    //Debug.Log(playerCounter);
                    AssignNextPlayer(i);
                }
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
            if (inPlayerSelect)
            {
                B4_P1_select.enabled = false;
                P1_selected.enabled = true;
                B4_P2_select.text = "Press A to be Player 2";
            }
        }

        if (playerTwoSelected)
        {
            if (inPlayerSelect)
            {
                B4_P2_select.enabled = false;
                P2_selected.enabled = true;
            }
        }

        //If players one and two were selected, then the bool becomes true
        if (playerOneSelected && playerTwoSelected)
        {
            bothPlayersReady = true;
            if (inPlayerSelect)
            {
                LevelStartBtnText.text = "Selecting Level...";
            }
            Debug.Log("Both players selected");
        }
    }

    //assign a player to a controller and change their joystick to the in game joystick
    public void AssignNextPlayer(int rewiredPlayerId)
    {
        if (playerCounter == 2)
        {
            rewiredPlayerId = 1;
            p1Controller = rewiredPlayerId;
        }
        else if (playerCounter == 1)
        {
            rewiredPlayerId = 0;
            p2Controller = rewiredPlayerId;
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

        //Debug.Log("rewiredPlayerId" + rewiredPlayerId);

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

    void CheckLevel()
    {
        if (SceneManager.GetActiveScene().name == "Player_Select")
        {
            InitializeScene();
            inPlayerSelect = true;
        }
        else
        {
            initialized = false;
            inPlayerSelect = false;

            playerOneSelected = false;
            playerTwoSelected = false;
            bothPlayersReady = false;
        }
    }

    private void InitializeScene()
    {
        if (!initialized)
        {
            Debug.Log("Initializing Scene");
            //Assigns object to appropriate spaces
            B4_P1_select = GameObject.Find("SelectP1").GetComponent<Text>();
            B4_P2_select = GameObject.Find("SelectP2").GetComponent<Text>();

            P1_selected = GameObject.Find("Selected_P1").GetComponent<Text>();
            P2_selected = GameObject.Find("Selected_P2").GetComponent<Text>();

            LevelStartBtn = GameObject.Find("StartGameBtn").GetComponent<Button>();
            LevelStartBtnText = GameObject.Find("StartGameBtnText").GetComponent<Text>();

            //Shows that none of the players are selected
            playerOneSelected = false;
            playerTwoSelected = false;
            bothPlayersReady = false;

            B4_P1_select.enabled = true;
            B4_P2_select.enabled = true;
            B4_P2_select.text = "Waiting for Player 1";


            //Hides the text
            P1_selected.enabled = false;
            P2_selected.enabled = false;

            LevelStartBtn.interactable = false;
            LevelStartBtnText.text = "Waiting for Players...";

            initialized = true;
            Debug.Log("Scene Initialized!");
        }
    }
}
