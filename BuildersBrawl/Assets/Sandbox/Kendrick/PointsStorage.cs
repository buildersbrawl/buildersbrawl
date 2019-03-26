using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsStorage : MonoBehaviour
{
    public static PointsStorage P;

    //stores all players
    public int[][] playersPoints;

    //stores all players' points in array
    public int[] P1Points = new int[4];
    public int[] P2Points = new int[4];
    public int[] P3Points = new int[4];
    public int[] P4Points = new int[4];

    //assigns point types to number in player array
    public int kills = 0;
    public int builds = 1;
    public int total = 2;
    public int wins = 3;

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
        //Don't destroy when going to next scene
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Reset data when in Main Menu scene
        if(SceneManager.GetActiveScene().name == "Main_Menu")
        {
            Reset();
            RoundsManager.R.Reset();
        }
    }

    void Reset()
    {
        for (int i = 0; i < P1Points.Length; i++)
        {
            P1Points[i] = 0;
            P2Points[i] = 0;
            P3Points[i] = 0;
            P4Points[i] = 0;

        }
    }
}
