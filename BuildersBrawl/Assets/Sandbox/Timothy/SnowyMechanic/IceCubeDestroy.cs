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
        Debug.Log(moveSpeed);
        Debug.Log(moveAmount);
        float timePassed = Time.time - startTimer;
        float movementPassed = 0;
        if (timePassed >= timeTilDestruction)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (movementPassed <= moveAmount)
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                movementPassed = transform.position.y - startPos.y;
            }
        }
    }
}
