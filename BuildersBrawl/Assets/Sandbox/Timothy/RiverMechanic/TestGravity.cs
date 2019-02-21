using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGravity : MonoBehaviour
{
    public float fallSpeed;
    public bool gravity = false;

    private void Update()
    {
        if (gravity)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
        /*
        if (!Physics.Raycast(transform.position, Vector3.down, 0.15f))
        {
            transform.position += Vector3.down * fallSpeed * Time.fixedDeltaTime;
        }*/
    }
}
