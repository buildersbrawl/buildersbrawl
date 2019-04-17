using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnArrows : MonoBehaviour
{
    private GameObject respawnArrow;

    private void Start()
    {
        respawnArrow = GetComponentInChildren<ArrowIdentifier>().gameObject;
    }

    private void Update()
    {
        if (!(this.GetComponent<PlayerDeath>().playerDead))
        {
            respawnArrow.SetActive(false);
        }
        else
        {
            respawnArrow.SetActive(true);
        }
    }
}
