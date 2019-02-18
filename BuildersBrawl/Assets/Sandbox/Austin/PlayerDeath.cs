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

    private Renderer playerRenderer;
    public Transform spawnPoint;
    public float respawnTime = 5f;

    [HideInInspector]
    public bool playerDead;

    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = this.GetComponent<Renderer>();

        playerDead = false;
        //target = Waypoints.points[0];
    }


    public void KillMe()
    {
        playerRenderer.enabled = false;
        this.gameObject.transform.position = spawnPoint.transform.position;
        print(this.gameObject.transform.position + " " + this.gameObject.name);
        playerDead = true;
        StartCoroutine(WaitForRenderer());

    }

    IEnumerator WaitForRenderer()
    {
        yield return new WaitForSeconds(respawnTime);
        playerRenderer.enabled = true;
        playerDead = false;
        print(this.gameObject.transform.position + " " + this.gameObject.name);
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
