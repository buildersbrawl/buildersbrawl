using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{

    public static int pointsTotal = 0;
    public int pointsForKill = 30;
    public int pointsForBoardPlace = 10;
    public int pointsForOtherSide = 200;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //if they are pushed and they die, based on time?
    public void AddPointsForKill()
    {
        Debug.Log("Points given for a kill");
        pointsTotal += pointsForKill;
    }

    public void AddPointsForBoardPlace()
    {
        Debug.Log("Points given for a board place");
        pointsTotal += pointsForBoardPlace;
    }

    public void AddPointsForOtherSide()
    {
        Debug.Log("Points given for a win");
        pointsTotal += pointsForOtherSide;
    }
}
