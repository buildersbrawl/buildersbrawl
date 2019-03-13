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

    private bool P4 = true, P3 = true;

    private int title = 0;
    private int kills = 1;
    private int builds = 2;
    private int points = 3;
    private int wins = 4;

    //Stores player colors
    public Material P1_Color, P2_Color, P3_Color, P4_Color;

    public string first, second, third, last;

    //Stores Player's points, name, and image to calculate who won
    public int[] playerPoints;
    public string[] playerName;
    public Sprite[][] playerFaces;
    public Image[] playerImages;

    public Sprite[] P1Faces = new Sprite[3];
    public Sprite[] P2Faces = new Sprite[3];
    public Sprite[] P3Faces = new Sprite[3];
    public Sprite[] P4Faces = new Sprite[3];

    void Awake()
    {
        //Assigns appropriate player color to panels
        P1Data.GetComponent<Image>().color = P1_Color.color;
        P2Data.GetComponent<Image>().color = P2_Color.color;
        P3Data.GetComponent<Image>().color = P3_Color.color;
        P4Data.GetComponent<Image>().color = P4_Color.color;
        CheckPlayerNumbers();

        playerPoints = new int[4];
       /* playerPoints[0] = PointsStorage.P.P1Points[PointsStorage.P.total];
        playerPoints[1] = PointsStorage.P.P2Points[PointsStorage.P.total];
        playerPoints[2] = PointsStorage.P.P3Points[PointsStorage.P.total];
        playerPoints[3] = PointsStorage.P.P4Points[PointsStorage.P.total];*/

        playerImages = new Image[4];
        playerImages[0] = P1Data.GetComponentInChildren<Image>(title);
        playerImages[1] = P2Data.GetComponentInChildren<Image>(title);
        playerImages[2] = P3Data.GetComponentInChildren<Image>(title);
        playerImages[3] = P4Data.GetComponentInChildren<Image>(title);

        playerFaces = new Sprite[4][];
        playerFaces[0] = P1Faces;
        playerFaces[1] = P2Faces;
        playerFaces[2] = P3Faces;
        playerFaces[3] = P4Faces;

        playerName = new string[4];
        playerName[0] = "Player 1";
        playerName[1] = "Player 2";
        playerName[2] = "Player 3";
        playerName[3] = "Player 4";
    }

    // Update is called once per frame
    void Update()
    {
        CompareTotals();
        //Displays player points
        //DisplayPoints();

        /*Debug.Log("First Place: " + first);
        Debug.Log("Second Place: " + second);
        Debug.Log("Third Place: " + third);
        Debug.Log("Last Place: " + last);*/
    }

    void CheckPlayerNumbers()
    {
        players = new int[4];
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
        P4Data.GetComponentInChildren<Image>(title).enabled = false;
        P4Data.GetComponentInChildren<Text>(kills).text = "";
        P4Data.GetComponentInChildren<Text>(builds).text = "";
        P4Data.GetComponentInChildren<Text>(points).text = "";
        P4Data.GetComponentInChildren<Text>(wins).text = "";
        P4Data.GetComponent<Image>().color = Color.gray;
        P4 = false;
    }

    void DisablePlayer3()
    {
        P3Data.GetComponentInChildren<Image>(title).enabled = false;
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
        int[] place = new int[4];
        int prev = 0;

            //Calculate first place
            for (int f = 0; f < 4; f++)
            {
            
                if (place[0] < playerPoints[f])
                {
                    place[0] = playerPoints[f];
                    first = playerName[f];
                playerImages[prev].sprite = playerFaces[prev][2];
                playerImages[f].sprite = playerFaces[f][1];
                prev = f;
                }
            }
        prev = 0;
            //Calculate second place
            for (int f = 0; f < 4; f++)
            {
                if (place[0] > playerPoints[f])
                {
                    if (place[1] < playerPoints[f])
                    {
                        place[1] = playerPoints[f];
                        second = playerName[f];
                    playerImages[prev].sprite = playerFaces[prev][2];
                    playerImages[f].sprite = playerFaces[f][0];
                    prev = f;
                    if (!P3)
                        {
                            last = second;
                            playerImages[f].sprite = playerFaces[f][2];
                    }
                    }
                }
            }
        /*if (!P3)
        {
            last = second;

        }*/
        prev = 0;
        if (P3)
        {
            //Calculate third place
            for (int f = 0; f < 4; f++)
            {
                if (place[0] > playerPoints[f] && place[1] > playerPoints[f])
                {
                    if (place[2] < playerPoints[f])
                    {
                        place[2] = playerPoints[f];
                        third = playerName[f];
                        playerImages[prev].sprite = playerFaces[prev][2];
                        playerImages[f].sprite = playerFaces[f][0];
                        prev = f;
                        if (!P4)
                        {
                            last = third;
                            playerImages[f].sprite = playerFaces[f][2];
                        }
                    }
                }
            }
           /* if (!P4)
            {
                last = third;
            }*/
            if (P4)
            {

                //Calculate forth place
                for (int f = 0; f < 4; f++)
                {
                    if (place[0] < playerPoints[f] && place[1] < playerPoints[f] && place[2] < playerPoints[f])
                    {
                        if (place[3] < playerPoints[f])
                        {
                            place[3] = playerPoints[f];
                            last = playerName[f];
                            playerImages[prev].sprite = playerFaces[prev][2];
                            playerImages[f].sprite = playerFaces[f][2];
                            prev = f;
                        }
                    }
                }
            }
        }
    }
}
