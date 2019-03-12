using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyMechanic : MonoBehaviour
{
    public GameObject iceCubePrefab;
    private Vector3 deathPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            deathPos = other.gameObject.transform.position + new Vector3(0, -1.1f);
            //Debug.Log("DeathPos: " + deathPos); //12.3,5.3,90.5
            other.gameObject.GetComponent<PlayerDeath>().KillMe();
            Instantiate(iceCubePrefab, deathPos, Quaternion.identity);
        }
    }
}
