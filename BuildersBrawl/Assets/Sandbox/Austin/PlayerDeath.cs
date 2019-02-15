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
        playerRenderer = GetComponent<Renderer>();
        gravity = oldGravity;
        if (!isRigidbody)
        {
            cController = GetComponent<CharacterController>();
            moveForward = transform.TransformDirection(Vector3.forward);
            
        }
            
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
            //transform.LookAt(spawnPoint);

            transform.position = spawnPoint.transform.position;
            deathHappened = false;
            StartCoroutine(WaitForRenderer());
            
            
            /*if (Vector3.Distance(transform.position, spawnPoint.position) >= minDistance && playerDeathNumber == playerNumber)
            {
                transform.position += transform.forward * deathMoveSpeed * Time.deltaTime;
                transform.Translate(transform.position - spawnPoint.position, Space.World);
            }
            else
            {
                playerRenderer.enabled = true;
                deathHappened = false;
            }*/
        }
    }  
}
