using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject spawnPoint;
    public float waitTime = 5f;
    private Renderer playerRenderer;
    public float deathMoveSpeed = 10f;
    private Transform target;
    private int pointIndex = 0;
    public static bool deathHappened = false;
    

    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        playerRenderer.enabled = true;

        target = Waypoints.points[0];
    }

    //once they die, wait 5 seconds, have player be there but not visible
    IEnumerator DeathWait()
    {
        //turn off the player's mesh renderer
        playerRenderer.enabled = false;
        yield return new WaitForSeconds(waitTime);
        //turn on player's mesh renderer
        playerRenderer.enabled = true;
    }

    //when it contacts a 
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DeathObject")
        {
            deathHappened = true;
            StartCoroutine(DeathWait());
            //other.enabled = false;
            
        }
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        //start coroutine for the player's renderer to turn off then on
        //StartCoroutine(DeathWait());

        //move towards the waypoint
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * deathMoveSpeed * Time.deltaTime, Space.World);

        //if they are close to the current waypoint, get the next waypoint
        if(Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
        
    }

    //get the next waypoint in the array and set it as the target
    void GetNextWaypoint()
    {

        if(pointIndex >= Waypoints.points.Length - 1)
        {
            Debug.Log("Reached the end");
            return;
        }
        pointIndex++;
        target = Waypoints.points[pointIndex];
    }
}
