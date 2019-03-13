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
    public Sprite[] faces = new Sprite[3];
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
        AddPlayerKill();
    }

    public void AddPointsForBoardPlace()
    {
        Debug.Log("Points given for a board place");
        pointsTotal += pointsForBoardPlace;
        PrintPointsTotal();
        AddPlayerBuild();
    }

    public void AddPointsForOtherSide()
    {
        Debug.Log("Points given for a win");
        pointsTotal += pointsForOtherSide;
        PrintPointsTotal();
        AddPlayerWin();
    }

    public int GetPointsTotal()
    {
        return pointsTotal;
    }

    private void PrintPointsTotal()
    {
        Debug.Log(this.name + " has " + pointsTotal + " total points");
    }

    public Sprite GetFace()
    {
        return faces[activeFaceNum];
    }

    public void ChangeFaceNum(int newNum)
    {
        activeFaceNum = newNum;
    }

    //SAVE POINTS TO PointsStorage

    //Save Kill Points
    void AddPlayerKill()
    {
        if(this.gameObject.name.Equals("PlayerPrefab_P1"))
        {
            PointsStorage.P.P1Points[PointsStorage.P.kills] ++;
            PointsStorage.P.P1Points[PointsStorage.P.total] += pointsForKill;
        }
        if (this.gameObject.name.Equals("PlayerPrefab_P2"))
        {
            PointsStorage.P.P2Points[PointsStorage.P.kills] ++;
            PointsStorage.P.P2Points[PointsStorage.P.total] += pointsForKill;
        }
    }

    //Save Build Points
    void AddPlayerBuild()
    {
        if (this.gameObject.name.Equals("PlayerPrefab_P1"))
        {
            PointsStorage.P.P1Points[PointsStorage.P.builds] ++;
            PointsStorage.P.P1Points[PointsStorage.P.total] += pointsForBoardPlace;
        }
        if (this.gameObject.name.Equals("PlayerPrefab_P2"))
        {
            PointsStorage.P.P2Points[PointsStorage.P.builds] ++;
            PointsStorage.P.P2Points[PointsStorage.P.total] += pointsForBoardPlace;
        }
    }

    //Save Win Points and total Wins
    void AddPlayerWin()
    {
        if (this.gameObject.name.Equals("PlayerPrefab_P1"))
        {
            PointsStorage.P.P1Points[PointsStorage.P.total] += pointsForOtherSide;
            PointsStorage.P.P1Points[PointsStorage.P.wins] ++;
        }
        if (this.gameObject.name.Equals("PlayerPrefab_P2"))
        {
            PointsStorage.P.P2Points[PointsStorage.P.total] += pointsForOtherSide;
            PointsStorage.P.P2Points[PointsStorage.P.wins]++;
        }
    }
}
