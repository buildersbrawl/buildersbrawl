using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointsTEST : MonoBehaviour
{
    public int pointsTotal = 0;
    public int pointsForKill = 30;
    public int pointsForBoardPlace = 10;
    public int pointsForOtherSide = 200;

    public Text round;

    //if they are pushed and they die, based on time?
    public void AddPointsForKill(GameObject button)
    {
        Debug.Log("Points given for a kill");
        pointsTotal += pointsForKill;
        PrintPointsTotal();
        AddPlayerKill(button);
    }

    public void AddPointsForBoardPlace(GameObject button)
    {
        //Debug.Log("Points given for a board place");
        pointsTotal += pointsForBoardPlace;
        PrintPointsTotal();
        AddPlayerBuild(button);
    }

    public void AddPointsForOtherSide(GameObject button)
    {
        //Debug.Log("Points given for a win");
        pointsTotal += pointsForOtherSide;
        PrintPointsTotal();
        AddPlayerWin(button);
    }

    public int GetPointsTotal()
    {
        return pointsTotal;
    }

    private void PrintPointsTotal()
    {
        Debug.Log(this.name + " has " + pointsTotal + " total points");
    }

    //SAVE POINTS TO PointsStorage

    //Save Kill Points
    public void AddPlayerKill(GameObject button)
    {
        Debug.Log("Add kill");
        if (button.CompareTag("Player1"))
        {
            Debug.Log("Add P1 kill");
            PointsStorageTest.T.P1Points[PointsStorageTest.T.kills]++;
            PointsStorageTest.T.P1Points[PointsStorageTest.T.total] += pointsForKill;
        }
        if (button.CompareTag("Player2"))
        {
            PointsStorageTest.T.P2Points[PointsStorageTest.T.kills]++;
            PointsStorageTest.T.P2Points[PointsStorageTest.T.total] += pointsForKill;
        }
        if (button.CompareTag("Player3"))
        {
            PointsStorageTest.T.P3Points[PointsStorageTest.T.kills]++;
            PointsStorageTest.T.P3Points[PointsStorageTest.T.total] += pointsForKill;
        }
        if (button.CompareTag("Player4"))
        {
            PointsStorageTest.T.P4Points[PointsStorageTest.T.kills]++;
            PointsStorageTest.T.P4Points[PointsStorageTest.T.total] += pointsForKill;
        }
    }

    //Save Build Points
    public void AddPlayerBuild(GameObject button)
    {
        if (button.CompareTag("Player1"))
        {
            PointsStorageTest.T.P1Points[PointsStorageTest.T.builds]++;
            PointsStorageTest.T.P1Points[PointsStorageTest.T.total] += pointsForBoardPlace;
        }
        if (button.CompareTag("Player2"))
        {
            PointsStorageTest.T.P2Points[PointsStorageTest.T.builds]++;
            PointsStorageTest.T.P2Points[PointsStorageTest.T.total] += pointsForBoardPlace;
        }
        if (button.CompareTag("Player3"))
        {
            PointsStorageTest.T.P3Points[PointsStorageTest.T.builds]++;
            PointsStorageTest.T.P3Points[PointsStorageTest.T.total] += pointsForBoardPlace;
        }
        if (button.CompareTag("Player4"))
        {
            PointsStorageTest.T.P4Points[PointsStorageTest.T.builds]++;
            PointsStorageTest.T.P4Points[PointsStorageTest.T.total] += pointsForBoardPlace;
        }
    }

    //Save Win Points and total Wins
    public void AddPlayerWin(GameObject button)
    {
        if (button.CompareTag("Player1"))
        {
            PointsStorageTest.T.P1Points[PointsStorageTest.T.total] += pointsForOtherSide;
            PointsStorageTest.T.P1Points[PointsStorageTest.T.wins]++;
        }
        if (button.CompareTag("Player2"))
        {
            PointsStorageTest.T.P2Points[PointsStorageTest.T.total] += pointsForOtherSide;
            PointsStorageTest.T.P2Points[PointsStorageTest.T.wins]++;
        }
        if (button.CompareTag("Player3"))
        {
            PointsStorageTest.T.P3Points[PointsStorageTest.T.total] += pointsForOtherSide;
            PointsStorageTest.T.P3Points[PointsStorageTest.T.wins]++;
        }
        if (button.CompareTag("Player4"))
        {
            PointsStorageTest.T.P4Points[PointsStorageTest.T.total] += pointsForOtherSide;
            PointsStorageTest.T.P4Points[PointsStorageTest.T.wins]++;
        }
    }

    public void CheckScore()
    {
        Debug.Log("Button pressed");
        SceneManager.LoadScene("EndGameTest");
    }

    void Update()
    {
        round.text = "Round " + (PointsStorageTest.T.round);
        if(PointsStorageTest.T.round >= PointsStorageTest.T.maxRounds)
        {
            round.text = "Final Round";
        }
    }
}
