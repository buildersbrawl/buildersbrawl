using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{

    public int pointsTotal = 0;
    public int pointsForKill = 30;
    public int pointsForBoardPlace = 10;
    public int pointsForOtherSide = 200;
    public Image[] faces = new Image[3];
    public int activeFaceNum = 1;


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
        PrintPointsTotal();
    }

    public void AddPointsForBoardPlace()
    {
        Debug.Log("Points given for a board place");
        pointsTotal += pointsForBoardPlace;
        PrintPointsTotal();
    }

    public void AddPointsForOtherSide()
    {
        Debug.Log("Points given for a win");
        pointsTotal += pointsForOtherSide;
        PrintPointsTotal();
    }

    public int GetPointsTotal()
    {
        return pointsTotal;
    }

    private void PrintPointsTotal()
    {
        Debug.Log(this.name + " has " + pointsTotal + " total points");
    }

    public Image GetFace()
    {
        return faces[activeFaceNum];
    }
}
