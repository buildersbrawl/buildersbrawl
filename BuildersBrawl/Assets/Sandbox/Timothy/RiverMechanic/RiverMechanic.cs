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
    public float riverSpeed;

    private Collider riverCollider;
    private GameObject player;

    private void Awake()
    {
        riverCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        /*
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hitInfo, .5f))
        {
            hitInfo.collider.gameObject.transform.position += GetRiverFlowDirection(riverDirection);
        }
        */
        /*
        if (Physics.BoxCast(riverCollider.bounds.center, riverCollider.bounds.extents, Vector3.up,
            out RaycastHit hitInfo, transform.rotation, 0.1f))
        {
            
            if (hitInfo.collider != null)
                player = hitInfo.collider.gameObject;
            hitInfo.collider.gameObject.GetComponent<TestGravity>().gravity = false;
            hitInfo.collider.gameObject.transform.position += GetRiverFlowDirection(riverDirection);
        }
        else
        {
            if (player != null)
                player.GetComponent<TestGravity>().gravity = true;
        }*/
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position += GetRiverFlowDirection(riverDirection);
    }
    private void OnCollisionStay(Collision collision)
    {
        //collision.gameObject.GetComponent<TestGravity>().gravity = false;
        collision.gameObject.transform.position += GetRiverFlowDirection(riverDirection);
    }*/

    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered!");
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Still Triggered!");
        other.gameObject.GetComponent<TestGravity>().gravity = false;
        other.gameObject.transform.position += GetRiverFlowDirection(riverDirection);
    }*/

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.size);
    }
}
