using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class ControllerAssigner : MonoBehaviour
{
    private static ControllerAssigner instance;

    public int maxPlayers = 2;

    /*private List<PlayerMap> playerMap;

    private int gamePlayerIdCounter = 0;

    public InputManager inputManagerInstance; // = gameObject.GetComponent<InputManager>();

    public bool controllerSelected = false;
    public int playerCounter = 0;

    //set up panels so the color can be changed once the controller is selected
    public Image controller1Image;
    public Image controller2Image;

    public static Rewired.Player GetRewiredPlayer(int gamePlayerId)
    {
        if (!Rewired.ReInput.isReady)
            return null;
        if (instance == null)
        {
            Debug.Log("Not initialized");
            return null;
        }
            
        for (int i = 0; i < instance.playerMap.Count; i++)
        {
            if (instance.playerMap[i].gamePlayerId == gamePlayerId)
                return ReInput.players.GetPlayer(instance.playerMap[i].rewiredPlayerId);
        }
        return null;
    }

    void Awake()
    {
        playerMap = new List<PlayerMap>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            //problem is with if statement
            Debug.Log(i);
            if (ReInput.players.GetPlayer(i).GetButtonDown("Submit") && controllerSelected == false)
            {
                playerCounter++;
                Debug.Log(playerCounter);
                AssignNextPlayer(i);
                
            }
            
        }
    }

    //assign a player to a controller and change their joystick to the in game joystick
    public void AssignNextPlayer(int rewiredPlayerId)
    {
        if(playerCounter != 1)
        {
            rewiredPlayerId = 1;
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
        //playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

        Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

        //detect that they are no longer using UI and are in the game now
        //rewiredPlayer.controllers.maps.SetMapsEnabled(false, "UI");
        //rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");

        Debug.Log("Added Rewired Player id " + rewiredPlayerId + " to game player " + gamePlayerId);

        //change joysticks
        //inputManagerInstance.ChangeControllerForGame(rewiredPlayer);
        

        inputManagerInstance.isUsingUI = false;
        inputManagerInstance.controllerSelected = true;
        Debug.Log("rewiredPlayerId" + rewiredPlayerId);
        if (rewiredPlayerId == 0)
            controller1Image = ChangeToRed(controller1Image, rewiredPlayerId);
        if (rewiredPlayerId == 1)
            controller2Image = ChangeToRed(controller2Image, rewiredPlayerId);

    }

    //increment the playercounter
    public int GetNextPlayerId()
    {
        return gamePlayerIdCounter++;
    }

    public Image ChangeToRed(Image toChange, int gamePlayerId)
    {
        Debug.Log("I'm here");
        if (gamePlayerId == 0)
        {
            toChange.color = Color.red;
        }
        else if (gamePlayerId == 1)
        {
            toChange.color =Color.red;
        }
        return toChange;
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
    }*/
}