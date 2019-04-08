using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeDestroy : MonoBehaviour
{
    public float moveSpeed = 5;
    public float moveAmount = 0.2f;
    private float startTimer;
    private float timeTilDestruction = 5;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        startTimer = Time.time;
    }
    private void Update()
    {
        float timePassed = Time.time - startTimer;
        float movementPassed = transform.position.y - startPos.y;

        if (timePassed >= timeTilDestruction)
        {
            Destroy(this.gameObject);
        }
        else if(movementPassed <= moveAmount)
        {
            /*
            if (movementPassed <= moveAmount)
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                movementPassed = transform.position.y - startPos.y;
            }*/
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            Debug.Log("MovementPassed: " + movementPassed);
            Debug.Log("MovementAmount: " + moveAmount);
        }
    }
}
