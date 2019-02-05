using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Node") && other.transform.parent.gameObject.GetComponent<Rigidbody>() != null
            && transform.parent.gameObject.GetComponent<Rigidbody>() == null)
        {
            Destroy(other.transform.parent.gameObject.GetComponent<Rigidbody>());
            other.transform.parent.position = new Vector3(other.transform.parent.position.x,
                transform.parent.GetComponent<Transform>().position.y + .001f,
                other.transform.parent.position.z);
        }
    }
}
