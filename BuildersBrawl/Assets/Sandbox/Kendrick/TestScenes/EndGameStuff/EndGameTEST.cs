using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*public static class ExtensionFunction
{
    public static T GetComponentInChildren<T>(this GameObject gameObject, int index)
    {
        return gameObject.transform.GetChild(index).GetComponent<T>();
    }
}*/

public class EndGameTEST : MonoBehaviour
{
    //Keeps track of the number of players in the game
    public int players;
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

    public bool endGame;
    private GameObject[] endGameScreen;

    public Text continueButton;
    public Text roundDisplay;

    public Image P1Crown, P2Crown, P3Crown, P4Crown;

    void Awake()
    {
        //Assigns appropriate player color to panels
        P1Data.GetComponent<Image>().color = P1_Color.color;
        P2Data.GetComponent<Image>().color = P2_Color.color;
        P3Data.GetComponent<Image>().color = P3_Color.color;
        P4Data.GetComponent<Image>().color = P4_Color.color;
        CheckPlayerNumbers();

        playerPoints = new int[4];
        //Debug.Log(playerPoints.Length);
        playerPoints[0] = PointsStorageTest.T.P1Points[PointsStorageTest.T.total];
        playerPoints[1] = PointsStorageTest.T.P2Points[PointsStorageTest.T.total];
        playerPoints[2] = PointsStorageTest.T.P3Points[PointsStorageTest.T.total];
        playerPoints[3] = PointsStorageTest.T.P4Points[PointsStorageTest.T.total];

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

        continueButton.text = "Start round " + (PointsStorageTest.T.round + 1);
        roundDisplay.text = "Round " + (PointsStorageTest.T.round) + " Results";

        P1Crown.gameObject.SetActive(false);
        P2Crown.gameObject.SetActive(false);
        P3Crown.gameObject.SetActive(false);
        P4Crown.gameObject.SetActive(false);
    }

    void Start()
    {
        //Checks to see if screen is for the end of a round or the end of the game
        CheckStatus();
    }

    // Update is called once per frame
    void Update()
    {
        CompareTotals();
        //Displays player points
        DisplayPoints();
        CheckLast();

        if (endGame)
        {
            AssignAward();
        }

        /*Debug.Log("First Place: " + first);
        Debug.Log("Second Place: " + second);
        Debug.Log("Third Place: " + third);
        Debug.Log("Last Place: " + last);*/
    }

    void CheckPlayerNumbers()
    {
        players = 4;
        if (players < 4)
        {
            DisablePlayer4();
        }
        if (players < 3)
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

    void CheckStatus()
    {

        if (PointsStorageTest.T.round >= PointsStorageTest.T.maxRounds)
        {
            endGame = true;
            continueButton.text = "Click here to start over.";
            roundDisplay.text = "Final Results";
        }

        endGameScreen = GameObject.FindGameObjectsWithTag("EndGame");
        if (!endGame)
        {
            for (int i = 0; i < endGameScreen.Length; i++)
            {
                endGameScreen[i].SetActive(false);
            }
        }
    }

    void DisplayPoints()
    {
        //Player 1 points
        P1Data.GetComponentInChildren<Text>(kills).text = PointsStorageTest.T.P1Points[PointsStorageTest.T.kills].ToString();
        P1Data.GetComponentInChildren<Text>(builds).text = PointsStorageTest.T.P1Points[PointsStorageTest.T.builds].ToString();
        P1Data.GetComponentInChildren<Text>(points).text = PointsStorageTest.T.P1Points[PointsStorageTest.T.total].ToString();
        P1Data.GetComponentInChildren<Text>(wins).text = PointsStorageTest.T.P1Points[PointsStorageTest.T.wins].ToString();

        //Player 2 points
        P2Data.GetComponentInChildren<Text>(kills).text = PointsStorageTest.T.P2Points[PointsStorageTest.T.kills].ToString();
        P2Data.GetComponentInChildren<Text>(builds).text = PointsStorageTest.T.P2Points[PointsStorageTest.T.builds].ToString();
        P2Data.GetComponentInChildren<Text>(points).text = PointsStorageTest.T.P2Points[PointsStorageTest.T.total].ToString();
        P2Data.GetComponentInChildren<Text>(wins).text = PointsStorageTest.T.P2Points[PointsStorageTest.T.wins].ToString();

        if (P3)
        {
            //Player 3 points
            P3Data.GetComponentInChildren<Text>(kills).text = PointsStorageTest.T.P3Points[PointsStorageTest.T.kills].ToString();
            P3Data.GetComponentInChildren<Text>(builds).text = PointsStorageTest.T.P3Points[PointsStorageTest.T.builds].ToString();
            P3Data.GetComponentInChildren<Text>(points).text = PointsStorageTest.T.P3Points[PointsStorageTest.T.total].ToString();
            P3Data.GetComponentInChildren<Text>(wins).text = PointsStorageTest.T.P3Points[PointsStorageTest.T.wins].ToString();

            if (P4)
            {
                //Player 4 points
                P4Data.GetComponentInChildren<Text>(kills).text = PointsStorageTest.T.P4Points[PointsStorageTest.T.kills].ToString();
                P4Data.GetComponentInChildren<Text>(builds).text = PointsStorageTest.T.P4Points[PointsStorageTest.T.builds].ToString();
                P4Data.GetComponentInChildren<Text>(points).text = PointsStorageTest.T.P4Points[PointsStorageTest.T.total].ToString();
                P4Data.GetComponentInChildren<Text>(wins).text = PointsStorageTest.T.P4Points[PointsStorageTest.T.wins].ToString();
            }
        }
    }

    void CompareTotals()
    {
        int[] place = new int[4];

        //Calculate first place
        for (int f = 0; f < 4; f++)
        {

            if (place[0] < playerPoints[f])
            {
                place[0] = playerPoints[f];
                first = playerName[f];
                playerImages[f].sprite = playerFaces[f][1];
            }
        }

        //Calculate second place
        for (int f = 0; f < 4; f++)
        {
            if (place[0] > playerPoints[f])
            {
                if (place[1] < playerPoints[f])
                {
                    place[1] = playerPoints[f];
                    second = playerName[f];
                    playerImages[f].sprite = playerFaces[f][0];
                    if (!P3)
                    {
                        last = second;
                        playerImages[f].sprite = playerFaces[f][2];
                    }
                }
            }
        }

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
                        playerImages[f].sprite = playerFaces[f][0];
                        if (!P4)
                        {
                            last = third;
                            playerImages[f].sprite = playerFaces[f][2];
                        }
                    }
                }
            }

            if (P4)
            {
                //Calculate forth place
                for (int f = 0; f < 4; f++)
                {
                    if (place[0] > playerPoints[f] && place[1] > playerPoints[f] && place[2] > playerPoints[f])
                    {
                        if (place[3] > playerPoints[f] || last == "")
                        {
                            place[3] = playerPoints[f];
                            last = playerName[f];
                            Debug.Log(last + " is in last");
                            playerImages[f].sprite = playerFaces[f][2];
                        }
                    }
                }
            }
        }
    }

    public void ContinueButton()
    {
        if (!endGame)
        {
            PointsStorageTest.T.round++;
            SceneManager.LoadScene("PointsTestScene");
        }
        if (endGame)
        {
            SceneManager.LoadScene("Main_Menu");
        }
    }

    void CheckLast()
    {
            if (last == "Player 1")
            {
                playerImages[0].sprite = playerFaces[0][2];
            }
            if (last == "Player 2")
            {
                playerImages[1].sprite = playerFaces[1][2];
            }
            if (last == "Player 3")
            {
                playerImages[2].sprite = playerFaces[2][2];
            }
            if (last == "Player 4")
            {
                playerImages[3].sprite = playerFaces[3][2];
            }
    }

    void AssignAward()
    {
        if(first == "Player 1")
        {
            P1Crown.gameObject.SetActive(true);
        }
        if (first == "Player 2")
        {
            P2Crown.gameObject.SetActive(true);
        }
        if (first == "Player 3")
        {
            P3Crown.gameObject.SetActive(true);
        }
        if (first == "Player 4")
        {
            P4Crown.gameObject.SetActive(true);
        }
    }
}
