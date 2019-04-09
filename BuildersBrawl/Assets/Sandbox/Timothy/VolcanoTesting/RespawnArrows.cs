using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnArrows : MonoBehaviour
{
    public GameObject respawnArrow;
    private void Update()
    {
        if (!(this.GetComponent<PlayerDeath>().playerDead))
        {
            respawnArrow.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            respawnArrow.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
