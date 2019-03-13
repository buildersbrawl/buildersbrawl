using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public PlayerController[] playerList;

    public bool someoneWon = false;

    public GameObject cameraRef;

    private void Awake()
    {
        if(S == null)
        {
            S = this;
        }
        else
        {
            Destroy(this);
        }

        playerList = GameObject.FindObjectsOfType<PlayerController>();

        if (player1 == null || player2 == null)
        {
            //make sure 2 players
            
            if(playerList.Length < 2 || playerList.Length > 4)
            {
                print("either to few or too many players");
            }
            else
            {
                player1 = playerList[0].gameObject;
                player2 = playerList[1].gameObject;
                player3 = playerList[2].gameObject;
                player4 = playerList[3].gameObject;
            }
        }
        
        if(cameraRef == null)
        {
            cameraRef = GameObject.FindObjectOfType<CameraController>().gameObject;
        }

    }

    public void RestartGame()
    {
        someoneWon = false;
        SceneManager.LoadScene("Main_Menu");
    }

}
