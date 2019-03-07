using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsStorage : MonoBehaviour
{
    public static PointsStorage P;

    public List<string> players = new List<string>();
    public int[] kills = new int[4];
    public int[] builds = new int[4];
    public int[] total = new int[4];
    public int[] wins = new int[4];

    void Awake()
    {
        //if another one of these get rid of this one
        if(PointsStorage.P == null)
        {
            P = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Player_Select")
        {
            CheckForPlayers();
        }
        if(SceneManager.GetActiveScene().name == "Main_Menu")
        {
            Reset();
        }
    }

    void CheckForPlayers()
    {
        if(PlayerSelect.S.playerOneSelected)
        {
            players.Add("P1");
        }
        if(PlayerSelect.S.playerTwoSelected)
        {
            players.Add("P2");
        }
    }

    void Reset()
    {
        players.Clear();
        for (int i = 0; i < kills.Length; i++)
        {
            kills[i] = 0;
            builds[i] = 0;
            total[i] = 0;
            wins[i] = 0;

        }
    }
}
