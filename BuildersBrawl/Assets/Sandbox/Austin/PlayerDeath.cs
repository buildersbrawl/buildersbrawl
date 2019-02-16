using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public int playerNumber = 0;
    private int playerDeathNumber = -1;
    public Transform spawnPoint;
    private Renderer playerRenderer;
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

    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = this.GetComponent<Renderer>();
        
            
        //target = Waypoints.points[0];
    }



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

    IEnumerator WaitForRenderer()
    {
        yield return new WaitForSeconds(waitTime);
        playerRenderer.enabled = true;
    }

    void Update()
    {


        if(deathHappened)
        {
            if(playerDeathNumber == 0)
            {
                transform.position = spawnPoint.transform.position;
                deathHappened = false;
                StartCoroutine(WaitForRenderer());
            
            }

            else if (playerDeathNumber == 1)
            {
                transform.position = spawnPoint.transform.position;
                deathHappened = false;
                StartCoroutine(WaitForRenderer());

            }
        }
    }  
}
