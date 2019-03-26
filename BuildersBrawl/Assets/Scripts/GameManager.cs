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

    public GameObject winner;
    public bool someoneWon = false;

    public GameObject cameraRef;

    public int round;
    public int maxRounds;
    public bool endGame = false;

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

        if (player1 == null || player2 == null || player3 == null || player4 == null)
        {
            //make sure 2 players
            
            if(playerList.Length < 2 || playerList.Length > 4)
            {
                print("either to few or too many players");
            }
            else
            {
                foreach(PlayerController player in playerList)
                {
                    switch (player.playerNumber)
                    {
                        case PlayerController.PlayerNumber.p1:
                            player1 = player.gameObject;
                            break;
                        case PlayerController.PlayerNumber.p2:
                            player2 = player.gameObject;
                            break;
                        case PlayerController.PlayerNumber.p3:
                            player3 = player.gameObject;
                            break;
                        case PlayerController.PlayerNumber.p4:
                            player4 = player.gameObject;
                            break;
                        default:
                            break;
                    }
                }
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
        round = 0;
        endGame = false;
    }

    public void CheckEnd()
    {
        if(round >= maxRounds)
        {
            endGame = true;
        }
    }

}
