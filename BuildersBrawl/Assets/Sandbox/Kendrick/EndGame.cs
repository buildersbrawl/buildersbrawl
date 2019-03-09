using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionFunction
{
    public static T GetComponentInChildren<T>(this GameObject gameObject, int index)
    {
        return gameObject.transform.GetChild(index).GetComponent<T>();
    }
}

public class EndGame : MonoBehaviour
{
    //Keeps track of the number of players in the game
    public int[] players;
    public GameObject P1Data, P2Data, P3Data, P4Data;

    private bool P4, P3 = true;

    private int title = 0;
    private int kills = 1;
    private int builds = 2;
    private int points = 3;
    private int wins = 4;

    //Stores player colors
    public Material P1_Color, P2_Color, P3_Color, P4_Color;

    void Awake()
    {
        //Assigns appropriate player color to panels
        P1Data.GetComponent<Image>().color = P1_Color.color;
        P2Data.GetComponent<Image>().color = P2_Color.color;
        P3Data.GetComponent<Image>().color = P3_Color.color;
        P4Data.GetComponent<Image>().color = P4_Color.color;
        CheckPlayerNumbers();
    }

    // Update is called once per frame
    void Update()
    {
        //Displays player points
        DisplayPoints();
        CompareTotals();
    }

    void CheckPlayerNumbers()
    {
        players = new int[PlayerSelect.S.playerCounter];
        if (players.Length < 4)
        {
            DisablePlayer4();
        }
        if (players.Length < 3)
        {
            DisablePlayer3();
        }
    }

    void DisablePlayer4()
    {
        P4Data.GetComponentInChildren<Text>(title).text = "";
        P4Data.GetComponentInChildren<Text>(kills).text = "";
        P4Data.GetComponentInChildren<Text>(builds).text = "";
        P4Data.GetComponentInChildren<Text>(points).text = "";
        P4Data.GetComponentInChildren<Text>(wins).text = "";
        P4Data.GetComponent<Image>().color = Color.gray;
        P4 = false;
    }

    void DisablePlayer3()
    {
        P3Data.GetComponentInChildren<Text>(title).text = "";
        P3Data.GetComponentInChildren<Text>(kills).text = "";
        P3Data.GetComponentInChildren<Text>(builds).text = "";
        P3Data.GetComponentInChildren<Text>(points).text = "";
        P3Data.GetComponentInChildren<Text>(wins).text = "";
        P3Data.GetComponent<Image>().color = Color.gray;
        P3 = false;
    }

    void DisplayPoints()
    {
        //Player 1 points
        P1Data.GetComponentInChildren<Text>(kills).text = PointsStorage.P.P1Points[PointsStorage.P.kills].ToString();
        P1Data.GetComponentInChildren<Text>(builds).text = PointsStorage.P.P1Points[PointsStorage.P.builds].ToString();
        P1Data.GetComponentInChildren<Text>(points).text = PointsStorage.P.P1Points[PointsStorage.P.total].ToString();
        P1Data.GetComponentInChildren<Text>(wins).text = PointsStorage.P.P1Points[PointsStorage.P.wins].ToString();

        //Player 2 points
        P2Data.GetComponentInChildren<Text>(kills).text = PointsStorage.P.P2Points[PointsStorage.P.kills].ToString();
        P2Data.GetComponentInChildren<Text>(builds).text = PointsStorage.P.P2Points[PointsStorage.P.builds].ToString();
        P2Data.GetComponentInChildren<Text>(points).text = PointsStorage.P.P2Points[PointsStorage.P.total].ToString();
        P2Data.GetComponentInChildren<Text>(wins).text = PointsStorage.P.P2Points[PointsStorage.P.wins].ToString();

        if(P3)
        {
            //Player 3 points
            P3Data.GetComponentInChildren<Text>(kills).text = PointsStorage.P.P3Points[PointsStorage.P.kills].ToString();
            P3Data.GetComponentInChildren<Text>(builds).text = PointsStorage.P.P3Points[PointsStorage.P.builds].ToString();
            P3Data.GetComponentInChildren<Text>(points).text = PointsStorage.P.P3Points[PointsStorage.P.total].ToString();
            P3Data.GetComponentInChildren<Text>(wins).text = PointsStorage.P.P3Points[PointsStorage.P.wins].ToString();

            if(P4)
            {
                //Player 4 points
                P4Data.GetComponentInChildren<Text>(kills).text = PointsStorage.P.P4Points[PointsStorage.P.kills].ToString();
                P4Data.GetComponentInChildren<Text>(builds).text = PointsStorage.P.P4Points[PointsStorage.P.builds].ToString();
                P4Data.GetComponentInChildren<Text>(points).text = PointsStorage.P.P4Points[PointsStorage.P.total].ToString();
                P4Data.GetComponentInChildren<Text>(wins).text = PointsStorage.P.P4Points[PointsStorage.P.wins].ToString();
            }
        }
    }
    
    void CompareTotals()
    {
        if(PointsStorage.P.P1Points[PointsStorage.P.total] > PointsStorage.P.P2Points[PointsStorage.P.total])
        {

        }
    }
}
