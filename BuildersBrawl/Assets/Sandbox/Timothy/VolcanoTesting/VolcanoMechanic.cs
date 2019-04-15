using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoMechanic : MonoBehaviour
{
    public float timeUntilErupt;
    public float movePerFrame; //Lava Movement Per Frame
    public bool eruptCooldown = true;

    private Vector3 lavaStartingPos;
    private float startTimer;


    private void Start()
    {
        lavaStartingPos = transform.position;
        startTimer = Time.time;
    }

    private void Update()
    {
        
        if (!eruptCooldown)
        {
            Debug.Log("Eruption In Progress!");
        }
        else
        {
            Debug.Log("The Volcano Lies Dormant.");
            float timePassed = Time.time - startTimer;

            if (timePassed >= timeUntilErupt)
            {
                eruptCooldown = false; //End the Cooldown (Volcano is now active!)
            }
        }
    }
}
