using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObject : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerDeath>() != null)
        {
            print("kill player " + other.gameObject.name);
            other.gameObject.GetComponent<PlayerDeath>().KillMe();

        }
        else if(other.gameObject.GetComponent<PlankManager>() != null)
        {
            //destroys boards that go below map
            Destroy(other.gameObject);
        }
    }
}
