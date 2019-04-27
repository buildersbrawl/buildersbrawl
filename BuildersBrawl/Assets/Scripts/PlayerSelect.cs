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
    public int p3Controller;
    public int p4Controller;

    private Controller controller1, controller2, controller3, controller4;
    //public static PlayerSelect PS;

    //Button & Text before the players are selected
    public Button P1_select;
    public Text B4_P1_select;
    public Button P2_select;
    public Text B4_P2_select;
    public Button P3_select;
    public Text B4_P3_select;
    public Button P4_select;
    public Text B4_P4_select;

    /*//Text after players are selected
    public Text P1_selected;
    public Text P2_selected;*/

    //Character Models
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    public Button LevelStartBtn;
    public Text LevelStartBtnText;

    //checks if players are selected
    public bool playerOneSelected;
    public bool playerTwoSelected;
    public bool playerThreeSelected;
    public bool playerFourSelected;

    public bool TwoPlayersReady;
    public bool ThreePlayersReady;
    public bool FourPlayersReady;


    //variables for assigning controller
    public int minPlayers = 2;
    public int maxPlayers = 4;
    private List<PlayerMap> playerMap;
    private int gamePlayerIdCounter = 0;
    public InputManager inputManagerInstance;
    public bool controllerSelected = false;
    public int playerCounter = 0;

    public LevelSelector ls;

    public bool inPlayerSelect = true;
    private bool initialized = false;

    //Tom: for testing
    public bool allowControllerSelection;

    public static Rewired.Player GetRewiredPlayer(int gamePlayerId)
    {
        if (!Rewired.ReInput.isReady)
            return null;
        if (S == null)
        {
            Debug.Log("Not initialized");
            return null;
        }

        for (int i = 0; i < S.playerMap.Count; i++)
        {
            if (S.playerMap[i].gamePlayerId == gamePlayerId)
                return ReInput.players.GetPlayer(S.playerMap[i].rewiredPlayerId);
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

    }

    // Start is called before the first frame update
    void Start()
    {
        //PS = this;

        //don't destory when going to next scene
        DontDestroyOnLoad(this.gameObject);

        //Shows that none of the players are selected
        playerOneSelected = false;
        playerTwoSelected = false;
        playerThreeSelected = false;
        playerFourSelected = false;
        TwoPlayersReady = false;
        ThreePlayersReady = false;
        FourPlayersReady = false;

        //Displays the text that shows up before the players are selected
        if (inPlayerSelect)
        {
            P1_select.gameObject.SetActive(true);
            P2_select.gameObject.SetActive(true);
            P3_select.gameObject.SetActive(true);
            P4_select.gameObject.SetActive(true);

            P1_select.interactable = true;
            P2_select.interactable = true;
            P3_select.interactable = true;
            P4_select.interactable = true;

            B4_P1_select.enabled = true;
            B4_P2_select.enabled = true;
            B4_P3_select.enabled = true;
            B4_P4_select.enabled = true;

            B4_P2_select.text = "Waiting for Player 1";
            B4_P3_select.text = "Waiting for Player 1";
            B4_P4_select.text = "Waiting for Player 1";


            /*//Hides the text
            P1_selected.enabled = false;
            P2_selected.enabled = false;*/

            //Hides the player models
            //player1.SetActive(false);
            //player2.SetActive(false);

            LevelStartBtn.interactable = false;
            LevelStartBtnText.text = "Waiting for Players...";


        }
    }

    // Update is called once per frame
    void Update()
    {
        //if testing and no roundmanager make one
        if (allowControllerSelection && RoundsManager.R == null)
        {
            this.gameObject.AddComponent<RoundsManager>();
        }

        CheckLevel();
        if (inPlayerSelect || allowControllerSelection)
        {
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                if (RoundsManager.R.round <= 1)
                {
                    //if someone hits "Submit" button (A)
                    if (ReInput.players.GetPlayer(i).GetButtonDown("Submit"))
                    {
                        //TODO: if controller isnt already attached to player add player

                        //if controller 1
                        if (ReInput.players.GetPlayer(i) == ReInput.players.GetPlayer(0))
                        {
                            SelectPlayerOne();
                        }
                        //if controller 2
                        else if (ReInput.players.GetPlayer(i) == ReInput.players.GetPlayer(1))
                        {
                            SelectPlayerTwo();
                        }
                        //if controller 3
                        else if (ReInput.players.GetPlayer(i) == ReInput.players.GetPlayer(2))
                        {
                            SelectPlayerThree();
                        }
                        //if controller 4
                        else if (ReInput.players.GetPlayer(i) == ReInput.players.GetPlayer(3))
                        {
                            SelectPlayerFour();
                        }

                        playerCounter++;

                        /*
                        //if player one selected and controller that hit submit is player 1
                        if(playerOneSelected && ReInput.players.GetPlayer(i) == ReInput.players.GetPlayer(0))
                        { }



                        //change ui to reflect a controller being selected
                        if (playerCounter == 1)
                            SelectPlayerOne();
                        else if (playerCounter == 2)
                            SelectPlayerTwo();
                        else if (playerCounter == 3)
                            SelectPlayerThree();
                        else if (playerCounter == 4)
                            SelectPlayerFour();
                        */

                        //Debug.Log(playerCounter);
                        AssignNextPlayer(i);
                    }
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

    public void SelectPlayerThree()
    {
        //Selects player two if not previously done and if player one was already selected
        if (!playerThreeSelected && playerTwoSelected)
        {
            playerThreeSelected = true;
            Debug.Log("Player Three selected");
            CheckSelectedPlayers();
        }
    }

    public void SelectPlayerFour()
    {
        //Selects player two if not previously done and if player one was already selected
        if (!playerFourSelected && playerThreeSelected)
        {
            playerFourSelected = true;
            Debug.Log("Player Four selected");
            CheckSelectedPlayers();
        }
    }

    public void CheckSelectedPlayers()
    {
        //Checks if players one and two have been selected
        if (playerOneSelected)
        {
            if (inPlayerSelect)
            {
                B4_P1_select.gameObject.SetActive(false);
                P1_select.gameObject.SetActive(false);
                //P1_selected.enabled = true;
                player1.SetActive(true);
                B4_P2_select.text = "Press Start";
                B4_P3_select.text = "Waiting for Player 2";
                B4_P4_select.text = "Waiting for Player 2";
            }
        }

        if (playerTwoSelected)
        {
            if (inPlayerSelect)
            {
                B4_P2_select.enabled = false;
                P2_select.gameObject.SetActive(false);
                //P2_selected.enabled = true;
                player2.SetActive(true);
                B4_P3_select.text = "Press Start";
                B4_P4_select.text = "Waiting for Player 3";
            }
        }

        if (playerThreeSelected)
        {
            if (inPlayerSelect)
            {
                B4_P3_select.enabled = false;
                P3_select.gameObject.SetActive(false);
                //P2_selected.enabled = true;
                player3.SetActive(true);
                B4_P4_select.text = "Press Start";
            }
        }

        if (playerFourSelected)
        {
            if (inPlayerSelect)
            {
                B4_P4_select.enabled = false;
                P4_select.gameObject.SetActive(false);
                //P2_selected.enabled = true;
                player4.SetActive(true);
            }
        }

        //If players one and two were selected, then the bool becomes true
        if (playerOneSelected && playerTwoSelected)
        {
            TwoPlayersReady = true;
            Debug.Log("Two players selected");
        }

        if(TwoPlayersReady && playerThreeSelected)
        {
            ThreePlayersReady = true;
            Debug.Log("Three players selected");
        }

        if (ThreePlayersReady && playerFourSelected)
        {
            FourPlayersReady = true;
            Debug.Log("Four players selected");
        }
    }

    //assign a player to a controller and change their joystick to the in game joystick
    public void AssignNextPlayer(int rewiredPlayerId)
    {
        if (playerCounter == 2)
        {
            rewiredPlayerId = 1;
            p2Controller = rewiredPlayerId;
        }
        else if (playerCounter == 1)
        {
            rewiredPlayerId = 0;
            p1Controller = rewiredPlayerId;
        }
        else if (playerCounter == 3)
        {
            rewiredPlayerId = 2;
            p3Controller = rewiredPlayerId;
        }
        else if (playerCounter == 4)
        {
            rewiredPlayerId = 3;
            p4Controller = rewiredPlayerId;
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
            //make it so if in player select starts rounds
            InitializeRounds();
            inPlayerSelect = true;
        }
        else
        {
            
                initialized = false;
                inPlayerSelect = false;

            /*if (RoundsManager.R.round == 3)
            {
                playerOneSelected = false;
                playerTwoSelected = false;
                playerThreeSelected = false;
                playerFourSelected = false;
                TwoPlayersReady = false;
                ThreePlayersReady = false;
                FourPlayersReady = false;
            }*/
        }
    }

    private void InitializeScene()
    {
        if (!initialized)
        {
            Debug.Log("Initializing Scene");
            //Assigns object to appropriate spaces
            P1_select = GameObject.Find("SelectP1").GetComponent<Button>();
            P2_select = GameObject.Find("SelectP2").GetComponent<Button>();
            P3_select = GameObject.Find("SelectP3").GetComponent<Button>();
            P4_select = GameObject.Find("SelectP4").GetComponent<Button>();

            B4_P1_select = GameObject.Find("Select1").GetComponent<Text>();
            B4_P2_select = GameObject.Find("Select2").GetComponent<Text>();
            B4_P3_select = GameObject.Find("Select3").GetComponent<Text>();
            B4_P4_select = GameObject.Find("Select4").GetComponent<Text>();

            /*P1_selected = GameObject.Find("Selected_P1").GetComponent<Text>();
            P2_selected = GameObject.Find("Selected_P2").GetComponent<Text>();*/

            player1 = GameObject.Find("P1_PS");
            player2 = GameObject.Find("P2_PS");
            player3 = GameObject.Find("P3_PS");
            player4 = GameObject.Find("P4_PS");

            LevelStartBtn = GameObject.Find("StartGameBtn").GetComponent<Button>();
            LevelStartBtnText = GameObject.Find("StartGameBtnText").GetComponent<Text>();

            if (RoundsManager.R.round == 1)
            {
                //Shows that none of the players are selected
                playerOneSelected = false;
                playerTwoSelected = false;
                playerThreeSelected = false;
                playerFourSelected = false;
                TwoPlayersReady = false;
                ThreePlayersReady = false;
                FourPlayersReady = false;

                B4_P1_select.enabled = true;
                B4_P2_select.enabled = true;
                B4_P3_select.enabled = true;
                B4_P4_select.enabled = true;

                P1_select.gameObject.SetActive(true);
                P2_select.gameObject.SetActive(true);
                P3_select.gameObject.SetActive(true);
                P4_select.gameObject.SetActive(true);

                B4_P2_select.text = "Waiting for Player 1";
                B4_P3_select.text = "Waiting for Player 1";
                B4_P4_select.text = "Waiting for Player 1";


                /*//Hides the text
                P1_selected.enabled = false;
                P2_selected.enabled = false;*/

                //Hides the player models
                player1.SetActive(false);
                player2.SetActive(false);
                player3.SetActive(false);
                player4.SetActive(false);

                LevelStartBtn.interactable = false;
                LevelStartBtnText.text = "Waiting for Players...";

                //reset player count
                playerCounter = 0;
            }

            initialized = true;
            Debug.Log("Scene Initialized!");
        }
    }

    private void InitializeRounds()
    {
        if (RoundsManager.R.round > 1)
        {
            B4_P1_select.enabled = false;
            B4_P2_select.enabled = false;
            B4_P3_select.enabled = false;
            B4_P4_select.enabled = false;

            P1_select.gameObject.SetActive(false);
            P2_select.gameObject.SetActive(false);
            P3_select.gameObject.SetActive(false);
            P4_select.gameObject.SetActive(false);

            if (!ThreePlayersReady)
            {
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(false);
                player4.SetActive(false); 
            }

            if (!FourPlayersReady)
            {
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
                player4.SetActive(false);
            }
        }
    }

}
