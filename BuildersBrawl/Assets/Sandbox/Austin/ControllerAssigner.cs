using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControllerAssigner : MonoBehaviour
{
    private static ControllerAssigner instance;

    public int maxPlayers = 2;

    private List<PlayerMap> playerMap;

    private int gamePlayerIdCounter = 0;

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
            if (ReInput.players.GetPlayer(i).GetButtonDown("Submit"))
            {
                //Debug.Log("ASSIGNING PLAYER. i = " + i + " playerCount = " + ReInput.players.playerCount );
                AssignNextPlayer(i);
            }
            
        }
    }

    //assign a player to a controller and change their joystick to the in game joystick
    void AssignNextPlayer(int rewiredPlayerId)
    {
        if(playerMap.Count >= maxPlayers)
        {
            Debug.Log("Max players");
            return;
        }

        int gamePlayerId = GetNextPlayerId();

        //set rewired player to next game player slot
        playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

        Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

        //change joysticks
        rewiredPlayer.controllers.maps.SetMapsEnabled(false, "UI");
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");
    }

    //increment the playercounter
    private int GetNextPlayerId()
    {
        return gamePlayerIdCounter++;
    }

    //class to map the rewired player id to game player id
    private class PlayerMap
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
