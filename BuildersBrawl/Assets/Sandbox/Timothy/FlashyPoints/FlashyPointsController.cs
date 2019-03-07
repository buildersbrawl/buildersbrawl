using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashyPointsController : MonoBehaviour
{
    public float textActiveTime = 10;
    public float moveSpeed;
    private int textActiveCount = 0;
    private Color textColor = Color.red;
    float startTimer;

    private void Start()
    {
        startTimer = Time.time;
    }

    private void Update()
    {
        float timePassed = Time.time - startTimer;
        if (timePassed <= textActiveTime)
        {
            transform.rotation = GameManager.S.cameraRef.transform.rotation;
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            transform.position += GameManager.S.cameraRef.transform.forward * moveSpeed * Time.deltaTime;
            textColor = GetComponent<MeshRenderer>().material.color;
            textColor.a += (-1*(timePassed / textActiveTime)) * Time.deltaTime;
            textColor = GetComponent<MeshRenderer>().material.color = textColor;
            textActiveCount++;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
