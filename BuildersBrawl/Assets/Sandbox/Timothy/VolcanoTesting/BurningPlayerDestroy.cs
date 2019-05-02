using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningPlayerDestroy : MonoBehaviour
{
    //public float jumpForce;
    private float startTimer;
    private float timeTilDestruction = 5;
    //RaycastHit hit;
    //private LayerMask mask;

    private void Start()
    {
        startTimer = Time.time;
        //GetComponent<Rigidbody>().AddForce(new Vector3(0, 1 * jumpForce, 0));
    }
    private void Update()
    {
        float timePassed = Time.time - startTimer;

        if (timePassed >= timeTilDestruction)
        {
            Destroy(this.transform.parent.gameObject);
        }
        
    }
}
