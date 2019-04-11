using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoMechanic : MonoBehaviour
{
    public float timeUntilErupt;
    public float movePerFrame; //Lava Movement Per Frame

    private Vector3 lavaStartingPos;

    private void Start()
    {
        lavaStartingPos = transform.position;
    }
}
