using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyMechanic : MonoBehaviour
{
    public GameObject[] iceCubePrefab;
    private Vector3 deathPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            deathPos = other.gameObject.transform.position + new Vector3(0, -1.5f);
            //Debug.Log("DeathPos: " + deathPos); //12.3,5.3,90.5
            other.gameObject.GetComponent<PlayerDeath>().KillMe();
            //Instantiate(iceCubePrefab, deathPos, Quaternion.identity);
            InstantiateIceCube(other.gameObject, deathPos);
        }
    }

    private void InstantiateIceCube(GameObject player, Vector3 deathPos)
    {
        if (player == GameManager.S.player1)//Blue
            Instantiate(iceCubePrefab[0], deathPos, Quaternion.identity);
        else if (player == GameManager.S.player2)//Red
            Instantiate(iceCubePrefab[1], deathPos, Quaternion.identity);
        else if (player == GameManager.S.player3)//Yellow
            Instantiate(iceCubePrefab[2], deathPos, Quaternion.identity);
        else if (player == GameManager.S.player4)//Purple
            Instantiate(iceCubePrefab[3], deathPos, Quaternion.identity);
    }
}
