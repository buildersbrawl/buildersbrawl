using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
        {
            if (hit.collider.tag == "Node")
            {
                Destroy(transform.parent.gameObject.GetComponent<Rigidbody>());
                //transform.parent.gameObject.GetComponent<Rigidbody>()
                transform.parent.position = new Vector3(transform.parent.position.x, hit.collider.transform.parent.GetComponent<Transform>().position.y, transform.parent.position.z);
            }
            
        }
    }
}
