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

    //GameObjects to hold each player 
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    private float pointsPercent = 0f;

    //array of heads
    private Image[] playerHeads = new Image[2];

    //array of the pl;ayer GameObjects
    private List<GameObject> players = new List<GameObject>();
    //private GameObject[] players;
    
    //array for each player's current pointstotal
    private int[] playersPoints;

    [SerializeField]
    private int totalGamePoints = 0;

    // Start is called before the first frame update
    void Start()
    {

        playerHeads[0] = player1Head;
        playerHeads[1] = player2Head;
        //playerHeads[2] = player3Head;
        //playerHeads[3] = player4Head;

        player1 = GameObject.Find("Player1");
        players.Add(player1);
        //players[0] = player1;
        player2 = GameObject.Find("Player2");
        players.Add(player2);
        //players[1] = player2;

        //if there is no player3 or player4, do not assign their gameobject
        if (GameObject.Find("Player3") != null)
        {
            player3 = GameObject.Find("Player3");
            players.Add(player3);
            //players[2] = player3;
        }
        if(GameObject.Find("Player4") != null)
        {
            player4 = GameObject.Find("Player4");
            players.Add(player4);
            //players[3] = player4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //reset totalGamePoints
        totalGamePoints = 0;

        //iterate through the players and add up their points
        foreach (GameObject player in players)
        {
            //Points pointsScript = player.GetComponent<Points>();
            totalGamePoints += player.GetComponent<Points>().pointsTotal;
        }

        foreach (GameObject player in players)
        {
            //set the index for each individual player
            int index = -1;
            Debug.Log("index before = " + index);
            if (player.name == "Player1")
                index = 0;
            else if (player.name == "Player2")
                index = 1;
            else if (player.name == "Player3")
                index = 2;
            else if (player.name == "Player4")
                index = 3;
            Debug.Log("Player name = " + player.name);
            Debug.Log("index after = " + index);
            
            //position on bar is the player's points as a percentage of the totalgamepoints
            pointsPercent = ((float)player.GetComponent<Points>().GetPointsTotal() / (float)totalGamePoints);

            Debug.Log("playerHeads[index].name = " + playerHeads[index].name);
            Vector3 before = playerHeads[index].transform.position;

            //set their position to the percent of the distance between the two points
            if(totalGamePoints > 0)
                playerHeads[index].transform.position = Vector3.Lerp(startPosition.position, endPosition.position, pointsPercent);
            
            Vector3 after = playerHeads[index].transform.position;
            if (before != after)
                Debug.Log("moved " + playerHeads[index].name + " from " + before + " to " + after);
            Debug.Log(player.name + "'s points total = " + (player.GetComponent<Points>().GetPointsTotal()));
            Debug.Log(player.name + "'s pointsPercent = " + pointsPercent);
            Debug.Log("totalGamePoints = " + totalGamePoints);
            
            
            
        }
        
        /*for (int index = 0; index <= players; index++)
        {
            Debug.Log("Made it into first loop");
            Points pointsScript = players[index].GetComponent<Points>();
            playersPoints[index] = pointsScript.GetPointsTotal();
            totalGamePoints += playersPoints[index];
        }

        for (int index = 0; index < players.Length; index++)
        {
            //position on bar is the player's points as a percentage of the totalgamepoints
            float pointsPercent = playersPoints[index] / totalGamePoints;

            //set their position to the percent of the distance between the two points
            Vector2.Lerp(startPosition.position, endPosition.position, pointsPercent);
            Debug.Log("totalGamePoints = " + totalGamePoints);
        }*/

        
    }
}
