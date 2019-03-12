using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMechanic : MonoBehaviour
{
    public enum RiverDirection
    {
        North,
        East,
        South,
        West
    }

    public RiverDirection riverDirection;
    public float riverSpeed = 10f;
    
    //Moves Player as long as they are touching the River Collider
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            other.gameObject.GetComponent<PlayerMovement>().SetEnvironmentMomentum(GetRiverFlowDirection(riverDirection));
        }
        else if(other.gameObject.GetComponent<PlankManager>() != null)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(GetRiverFlowDirection(riverDirection));
        }
    }

    //Finds River Flow Direction Based on Inspector
    private Vector3 GetRiverFlowDirection(RiverDirection direction)
    {
        Vector3 riverDirection = Vector3.zero;

        switch (direction)
        {
            case RiverDirection.North:
                riverDirection = Vector3.forward * Time.deltaTime * riverSpeed;
                break;
            case RiverDirection.East:
                riverDirection = Vector3.right * Time.deltaTime * riverSpeed;
                break;
            case RiverDirection.South:
                riverDirection = Vector3.back * Time.deltaTime * riverSpeed;
                break;
            case RiverDirection.West:
                riverDirection = Vector3.left * Time.deltaTime * riverSpeed;
                break;
            default:
                Debug.Log("This direction does not exist!");
                break;
        }
        return riverDirection;
    }
}
