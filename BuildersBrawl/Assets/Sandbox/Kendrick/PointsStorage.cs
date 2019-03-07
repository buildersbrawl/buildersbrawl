using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsStorage : MonoBehaviour
{
    public static PointsStorage P;

    //stores all players' points in array
    public int[] P1Points= new int[5];
    public int[] P2Points = new int[5];

    //assigns point types to number in player array
    public int kills = 0;
    public int builds = 1;
    public int total = 2;
    public int wins = 3;
    public int winPoints = 4;

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
        if(SceneManager.GetActiveScene().name == "Main_Menu")
        {
            Reset();
        }
    }

    void Reset()
    {
        for (int i = 0; i < P1Points.Length; i++)
        {
            P1Points[i] = 0;
            P2Points[i] = 0;

        }
    }
}
