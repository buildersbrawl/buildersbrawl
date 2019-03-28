using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsStorageTest : MonoBehaviour
{
    public static PointsStorageTest T;

    public int[][] playersPoints;

    public int[] P1Points = new int[4];
    public int[] P2Points = new int[4];
    public int[] P3Points = new int[4];
    public int[] P4Points = new int[4];

    public int kills = 0;
    public int builds = 1;
    public int total = 2;
    public int wins = 3;

    public int round;
    public int maxRounds;

    void Awake()
    {
        if(PointsStorageTest.T == null)
        {
            T = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        round = 1;
        maxRounds = 3;
    }

    void Start()
    {
        //Don't destroy when going to next scene
        DontDestroyOnLoad(this.gameObject);
    }
}
