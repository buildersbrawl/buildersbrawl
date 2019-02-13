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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DeathObject")
        {
            MovePlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

        //transform.position = spawnPoint.transform.position;
        
    }

    void GetNextWaypoint()
    {
        pointIndex++;
        target = Waypoints.points[pointIndex];
    }
}
