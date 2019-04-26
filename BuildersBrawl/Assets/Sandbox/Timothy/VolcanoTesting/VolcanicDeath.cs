using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicDeath : MonoBehaviour
{
    public GameObject[] burningPlayers;
    private Vector3 deathPos;

    
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            deathPos = other.gameObject.transform.position;
            other.gameObject.GetComponent<PlayerDeath>().KillMe();
            InstantiateBurningPlayer(other.gameObject, deathPos);
        }
    }

    /*
    private void OnParticleTrigger(GameObject other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            deathPos = other.gameObject.transform.position;
            other.gameObject.GetComponent<PlayerDeath>().KillMe();
            InstantiateBurningPlayer(other.gameObject, deathPos);
        }
    }*/

    private void InstantiateBurningPlayer(GameObject player, Vector3 deathPos)
    {
        if (player == GameManager.S.player1)//Blue
        {
            Instantiate(burningPlayers[0], deathPos + new Vector3(0, -1.1f, 0), Quaternion.identity);
        }
        //Instantiate(iceCubePrefab[0], deathPos, Quaternion.identity);
        else if (player == GameManager.S.player2)//Red
        {
            Instantiate(burningPlayers[1], deathPos + new Vector3(0, -1.1f, 0), Quaternion.identity);
        }
        //Instantiate(iceCubePrefab[1], deathPos, Quaternion.identity);
        else if (player == GameManager.S.player3)//Yellow
        {
            Instantiate(burningPlayers[2], deathPos + new Vector3(0, -1.1f, 0), Quaternion.identity);
        }
        //Instantiate(iceCubePrefab[2], deathPos, Quaternion.identity);
        else if (player == GameManager.S.player4)//Purple
        {
            Instantiate(burningPlayers[3], deathPos + new Vector3(0, -1.1f, 0), Quaternion.identity);
        }
    }
}
