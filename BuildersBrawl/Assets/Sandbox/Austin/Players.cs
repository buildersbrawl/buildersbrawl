using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class Players : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    //private static List<PlayerMap> playerMap;


    public static void AssignController(int rPlayerNum, int gPlayerNum)
    {
        //playerMap.Add(new PlayerMap(rPlayerNum, gPlayerNum));
        //Rewired.Player

        
            

    }

    void Awake()
    {
        //playerMap = new List<PlayerMap>();
        //if(SceneManager.GetActiveScene().name == )
    }

    private void Start()
    {
        if (this.tag == "Player1")
            player1 = GetComponent<GameObject>();
        else if (this.tag == "Player2")
            player2 = GetComponent<GameObject>();
    }
}
