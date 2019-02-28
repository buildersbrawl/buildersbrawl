using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    /*
    public int playerNumber = 0;
    private int playerDeathNumber = -1;
    public Transform spawnPoint;
    
    public float deathMoveSpeed = 10f;
    private Transform target;
    public static bool deathHappened = false;
    public float minDistance = .2f;
    public bool isRigidbody = false;
    public CharacterController cController;
    
    public float oldGravity = 10f;
    public float gravity;
    public Vector3 moveForward;
    public float waitTime = 5f;
    */

    PlayerController playerController;

    private Renderer playerRenderer;
    public Transform spawnPoint;
    public float respawnTime = 5f;
    public float timeToWaitAfterPushed = 0.5f;
    private GameObject otherPlayer;

    [HideInInspector]
    public bool playerDead;

    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.GetComponent<PlayerController>() != null)
        {
            playerController = this.gameObject.GetComponent<PlayerController>();
        }
        else
        {
            playerController = this.gameObject.AddComponent<PlayerController>();
        }

        playerRenderer = this.GetComponent<Renderer>();

        playerDead = false;

        if (this.name == "Player1")
        {
            otherPlayer = GameObject.Find("Player2");
        }
        else if(this.name == "Player2")
        {
            otherPlayer = GameObject.Find("Player1");
        }
    }


    public void KillMe()
    {
        //if holding plank drop it
        if(playerController.playerActions.HeldPlank != null)
        {
            playerController.playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.drop);
        }

        GameObject cc = GameObject.Find("Main Camera");

        playerRenderer.enabled = false;

        //set start avgposition/distance (before moved)
        cc.GetComponent<CameraController>().SetDeathStartValues();

        //move player
        this.gameObject.transform.position = spawnPoint.transform.position;

        //set end avgposition/distance (before moved)
        cc.GetComponent<CameraController>().SetDeathEndValues();

        print(this.gameObject.transform.position + " " + this.gameObject.name);
        playerDead = true;
        StartCoroutine(WaitForRenderer());

        //change the camera settings to lerp/move the camera towards the player respawn
        
        //cc.GetComponent<CameraController>().setCameraBasedOnPlayers = false;      //remove comment when the camera works
        cc.GetComponent<CameraController>().setCameraBasedOnPlayers = false;
        
        
    }

    IEnumerator WaitForRenderer()
    {
        yield return new WaitForSeconds(respawnTime);
        this.gameObject.transform.eulerAngles = Vector3.zero;
        playerRenderer.enabled = true;
        playerDead = false;
        print(this.gameObject.transform.position + " " + this.gameObject.name);
    }

    public IEnumerator WaitForDeathToHappen()
    {
        Debug.Log(playerDead);
        yield return new WaitForSeconds(timeToWaitAfterPushed);
        //if player dies after a push (which would trigger this function), give points to other player
        if (playerDead)
        {
            Debug.Log("Player was pushed and killed");
            otherPlayer.GetComponent<Points>().AddPointsForKill();
        }
        
    }

    /*
    //when it contacts a death object, turn the player's renderer off and allow update() to move the player
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DeathObject")
        {
            
            deathHappened = true;
            //GetComponent<Rigidbody>().useGravity = false;
            playerRenderer.enabled = false;
            playerDeathNumber = playerNumber;
        }

    }

    

    void Update()
    {


        if(deathHappened)
        {
            print("death happened");
            if(playerDeathNumber == 0)
            {
                this.gameObject.transform.position = spawnPoint.transform.position;
                deathHappened = false;
                StartCoroutine(WaitForRenderer());
            
            }

            else if (playerDeathNumber == 1)
            {
                this.gameObject.transform.position = spawnPoint.transform.position;
                deathHappened = false;
                StartCoroutine(WaitForRenderer());

            }
        }
    }  
    */
}
