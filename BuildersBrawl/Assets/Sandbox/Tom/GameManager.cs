using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public GameObject player1;
    public GameObject player2;

    public bool someoneWon = false;

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

        if(player1 == null || player2 == null)
        {
            //make sure 2 players
            PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
            if(players.Length != 2)
            {
                print("either to few or too many players");
            }
            else
            {
                player1 = players[0].gameObject;
                player2 = players[1].gameObject;
            }
        }
        

    }

    public void RestartGame()
    {
        someoneWon = false;
        SceneManager.LoadScene("Main_Menu");
    }

}


