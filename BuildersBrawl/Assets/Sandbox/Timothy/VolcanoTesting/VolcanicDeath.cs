using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicDeath : MonoBehaviour
{
    public GameObject[] burningPlayers;
    private Vector3 deathPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            deathPos = other.gameObject.transform.position;
            other.gameObject.GetComponent<PlayerDeath>().KillMe();
            InstantiateBurningPlayer(other.gameObject, deathPos);
        }
    }

    private void InstantiateBurningPlayer(GameObject player, Vector3 deathPos)
    {

    }
}
