using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOfAnimTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerAnimation>() != null)
        {
            other.gameObject.GetComponent<PlayerAnimation>().CallAnimTrigger("ToBlownOff");
        }
    }
}
