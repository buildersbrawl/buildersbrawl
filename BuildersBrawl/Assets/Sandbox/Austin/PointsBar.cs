using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsBar : MonoBehaviour
{
    //get each player
    //assign head to each player
    //move within clamped region with there location as a function of points/total game points

    //images of each player's head
    public Image player1Head;
    public Image player2Head;
    public Image player3Head;
    public Image player4Head;

    //start and end locations for the heads to travel between
    public Transform startPosition;
    public Transform endPosition;

    public Image empty;

    //GameObjects to hold each player 
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    private float[] oldPointPercent = new float[4];

    private float pointsPercent = 0f;

    //array of heads
    private Image[] playerHeads = new Image[4];

    //array of the pl;ayer GameObjects
    private List<GameObject> players = new List<GameObject>();
    //private GameObject[] players;

    //array for each player's current pointstotal
    private int[] playersPoints;

    [SerializeField]
    private int totalGamePoints = 0;

    private int loser, middleOne, middleTwo = -1;
    private int winner = 0;


    // Start is called before the first frame update
    void Start()
    {
        if(empty == null)
        {
            empty = GameObject.Find("Empty").GetComponent<Image>();
        }
        empty.transform.position = startPosition.transform.position;

        playerHeads[0] = player1Head;
        playerHeads[1] = player2Head;
        playerHeads[2] = player3Head;
        playerHeads[3] = player4Head;

        player1 = GameManager.S.player1;
        players.Add(player1);
        player1.GetComponent<Points>().ChangeFaceNum(0); //Tom: set face image
        //player1Head.sprite = player1.GetComponent<Points>().GetFace();
        //players[0] = player1;
        player2 = GameManager.S.player2;
        players.Add(player2);
        player2.GetComponent<Points>().ChangeFaceNum(1); //Tom: set face image
        //player2Head.sprite = player2.GetComponent<Points>().GetFace();
        //players[1] = player2;

        //if there is no player3 or player4, do not assign their gameobject
        if (GameManager.S.player3 != null)
        {
            
            player3 = GameManager.S.player3;
            players.Add(player3);
            player3.GetComponent<Points>().ChangeFaceNum(2); //Tom: set face image
            //player3Head.sprite = player3.GetComponent<Points>().GetFace();
            //players[2] = player3;
            if (player3.activeInHierarchy)
                player3Head.gameObject.SetActive(true);
            else
                player3Head.gameObject.SetActive(false);
            Debug.Log("Player3Head active: " + player3.activeInHierarchy);
        }
        else
            player3Head.gameObject.SetActive(false);
        if (GameManager.S.player4 != null)
        {
            player4 = GameManager.S.player4;
            players.Add(player4);
            player4.GetComponent<Points>().ChangeFaceNum(3); //Tom: set face image
            //player4Head.sprite = player4.GetComponent<Points>().GetFace();
            //players[3] = player4;
            if (player4.activeInHierarchy)
                player4Head.gameObject.SetActive(true);
            else
                player4Head.gameObject.SetActive(false);
            Debug.Log("Player4Head active: " + player4.activeInHierarchy);
        }
        else
            player4Head.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        /*player1Head = player1.GetComponent<Points>().faces[players[index].GetComponent<Points>().activeFaceNum];
        player2Head = player2.GetComponent<Points>().faces[players[index].GetComponent<Points>().activeFaceNum];
        if(players.Count > 2)
        {
            player3Head = player3.GetComponent<Points>().faces[player3.GetComponent<Points>().activeFaceNum];
            if (players.Count == 4)
            {
                player4Head = player4.GetComponent<Points>().faces[player4.GetComponent<Points>().activeFaceNum];
            }
        }*/

        //Debug.Log("player1Head = " + player1.GetComponent<Points>().faces[player1.GetComponent<Points>().activeFaceNum]);


        //reset totalGamePoints
        totalGamePoints = 1;

        //iterate through the players and add up their points
        foreach (GameObject player in players)
        {
            if(player != null)
            {
                //Points pointsScript = player.GetComponent<Points>();
                totalGamePoints += player.GetComponent<Points>().pointsTotal;
            }
        }

        foreach (GameObject player in players)
        {

            //set the index for each individual player
            int index = -1;
            //Debug.Log("index before = " + index);
            if (player == GameManager.S.player1)
                index = 0;
            else if (player == GameManager.S.player2)
                index = 1;
            else if (player == GameManager.S.player3)
                index = 2;
            else if (player == GameManager.S.player4)
                index = 3;
            //Debug.Log("Player name = " + player.name);
            //Debug.Log("index after = " + index);

            /*if (index == 0)
            {
                playerHeads[index].sprite = player1.GetComponent<Points>().GetFace();
                //Debug.Log("GOTTEN FACE = " + players[index].GetComponent<Points>().GetFace() + 
                    //" and facenum = " + players[index].GetComponent<Points>().activeFaceNum);
            }
            if (index == 1)
            {
                player2Head.sprite = player2.GetComponent<Points>().GetFace();
            }
            if (index == 2)
            {
                player3Head.sprite = player3.GetComponent<Points>().GetFace();

            }
            if (index == 3)
            {
                player4Head.sprite = player4.GetComponent<Points>().GetFace();
            }
            */

            //determine the face to give the points bar
            if (player.GetComponent<Points>().GetPointsTotal() != 0)
            {
                //Debug.Log("face before = " + players[index].GetComponent<Points>().activeFaceNum);
                DetermineFace(players[index]);
                //Debug.Log("face after = " + players[index].GetComponent<Points>().activeFaceNum);
                //Debug.Log("face name = " + players[index].GetComponent<Points>().GetFace());
            }


            //position on bar is the player's points as a percentage of the totalgamepoints
            pointsPercent = ((float)player.GetComponent<Points>().GetPointsTotal() / (float)totalGamePoints);


            //Debug.Log("playerHeads[index].name = " + playerHeads[index].name);
            Vector3 before = playerHeads[index].transform.position;

            //set their position to the percent of the distance between the two points
            if (oldPointPercent[index] != pointsPercent)
            {
                //Debug.Log("oldpoint[index] = " + oldPointPercent[index]);
                //Debug.Log("pointspercent = " + pointsPercent);

                //playerHeads[index].transform.position = Vector3.Lerp(startPosition.position, endPosition.position, Mathf.Lerp(0, pointsPercent, .05f));
                print(pointsPercent);
                empty.transform.position = Vector3.Lerp(startPosition.transform.position, endPosition.transform.position, pointsPercent);
                playerHeads[index].transform.position = Vector3.Lerp(playerHeads[index].transform.position, empty.transform.position, 0.05f);
            }

            Vector3 after = playerHeads[index].transform.position;

            if (before == after)
            {
                //oldPointPercent[index] = pointsPercent;
                //Debug.Log("BEFORE = AFTER");
            }



            /*if (before != after)
                Debug.Log("moved " + playerHeads[index].name + " from " + before + " to " + after);
            Debug.Log(player.name + "'s points total = " + (player.GetComponent<Points>().GetPointsTotal()));
            Debug.Log(player.name + "'s pointsPercent = " + pointsPercent);
            Debug.Log("totalGamePoints = " + totalGamePoints);
            */


        }

    }

    void DetermineFace(GameObject p)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].GetComponent<Points>().GetPointsTotal() >= winner)
            {
                winner = players[i].GetComponent<Points>().GetPointsTotal();
                Debug.Log("WINNER HAS " + winner);
            }
            else if (players[i].GetComponent<Points>().GetPointsTotal() < winner)
            {
                if(players.Count == 2)
                {
                    loser = players[i].GetComponent<Points>().GetPointsTotal();
                    Debug.Log("LOSER HAS " + loser);
                }
                else if(players.Count == 3 && players[i].GetComponent<Points>().GetPointsTotal() < middleOne)
                {
                    loser = players[i].GetComponent<Points>().GetPointsTotal();
                    Debug.Log("LOSER HAS " + loser);
                }
                else if(players.Count == 4 && players[i].GetComponent<Points>().GetPointsTotal() < middleOne 
                    && players[i].GetComponent<Points>().GetPointsTotal() < middleTwo)
                {
                    loser = players[i].GetComponent<Points>().GetPointsTotal();
                    Debug.Log("LOSER HAS " + loser);
                }
            }

            else if (players[i].GetComponent<Points>().GetPointsTotal() < winner && players.Count == 2)
            {
                loser = players[i].GetComponent<Points>().GetPointsTotal();
                Debug.Log("LOSER HAS " + loser);
            }
            else if (players[i].GetComponent<Points>().GetPointsTotal() < winner && players.Count == 3 && players[i].GetComponent<Points>().GetPointsTotal() < middleOne )
            {
                loser = players[i].GetComponent<Points>().GetPointsTotal();
                Debug.Log("LOSER HAS " + loser);
            }

            else if(players[i].GetComponent<Points>().GetPointsTotal() > loser && players[i].GetComponent<Points>().GetPointsTotal() < winner)
            {
                if(players.Count >= 3 && players[i].GetComponent<Points>().GetPointsTotal() > middleTwo)
                {
                    middleOne = players[i].GetComponent<Points>().GetPointsTotal();
                }
                else if(players.Count == 4 && players[i].GetComponent<Points>().GetPointsTotal() < middleOne)
                {
                    middleTwo = players[i].GetComponent<Points>().GetPointsTotal();
                }
            }
            
        }

        //Image tempFace;
        //Debug.Log(p.name + " has " + p.GetComponent<Points>().GetPointsTotal() + " points in DetermineFace");
        Debug.Log("winner has " + winner + " points in DetermineFace");
        Debug.Log("loser has " + loser + " points in DetermineFace");
        //Debug.Log("face before = " + p.GetComponent<Points>().activeFaceNum);

        if (p.GetComponent<Points>().GetPointsTotal() >= winner)
        {
            //p.GetComponent<Points>().activeFaceNum = 2;
            //p.GetComponent<Points>().ChangeFaceNum(2);
            winner = p.GetComponent<Points>().GetPointsTotal();
            Debug.Log(p.name + "is the new winner");
        }

        else if (p.GetComponent<Points>().GetPointsTotal() <= loser)
        {
            //p.GetComponent<Points>().activeFaceNum = 0;
            loser = p.GetComponent<Points>().GetPointsTotal();
            Debug.Log(p.name + "is the new loser");
        }

        else if (players.Count > 2 && p.GetComponent<Points>().GetPointsTotal() > loser &&
            p.GetComponent<Points>().GetPointsTotal() < winner)
        {
            if (players.Count == 3 || p.GetComponent<Points>().GetPointsTotal() > middleOne)
            {
                //p.GetComponent<Points>().activeFaceNum = 3;
                middleOne = p.GetComponent<Points>().GetPointsTotal();
                Debug.Log(p.name + "is the new middleOne");
            }
            else if (players.Count == 4 && p.GetComponent<Points>().GetPointsTotal() < middleOne)
            {
                //p.GetComponent<Points>().activeFaceNum = 4;
                middleTwo = p.GetComponent<Points>().GetPointsTotal();
                Debug.Log(p.name + "is the new middleTwo");
            }

            //Debug.Log("face after = " + p.GetComponent<Points>().activeFaceNum);
        }
    }
}
