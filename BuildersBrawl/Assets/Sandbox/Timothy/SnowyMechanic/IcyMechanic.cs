using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyMechanic : MonoBehaviour
{
    //public GameObject[] iceCubePrefab;
    public PlayerIceCubes[] playerIceCubePrefabs;
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
        {
            Instantiate(playerIceCubePrefabs[0].iceCubePrefabs[UnityEngine.Random.Range(0, 2)], deathPos, Quaternion.identity);
        }
        //Instantiate(iceCubePrefab[0], deathPos, Quaternion.identity);
        else if (player == GameManager.S.player2)//Red
        {
            Instantiate(playerIceCubePrefabs[1].iceCubePrefabs[UnityEngine.Random.Range(0, 2)], deathPos, Quaternion.identity);
        }
        //Instantiate(iceCubePrefab[1], deathPos, Quaternion.identity);
        else if (player == GameManager.S.player3)//Yellow
        {
            Instantiate(playerIceCubePrefabs[2].iceCubePrefabs[UnityEngine.Random.Range(0, 2)], deathPos, Quaternion.identity);
        }
        //Instantiate(iceCubePrefab[2], deathPos, Quaternion.identity);
        else if (player == GameManager.S.player4)//Purple
        {
            Instantiate(playerIceCubePrefabs[3].iceCubePrefabs[UnityEngine.Random.Range(0, 2)], deathPos, Quaternion.identity);
        }
            //Instantiate(iceCubePrefab[3], deathPos, Quaternion.identity);
    }
}

[System.Serializable]
public class PlayerIceCubes
{
    public GameObject[] iceCubePrefabs;
}