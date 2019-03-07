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
    private float alpha;
    float startTimer;

    private void Start()
    {
        //textColor = GetComponent<TextMesh>().color;
        //StartCoroutine(FadeText());
        startTimer = Time.time;
    }

    private void Update()
    {
        float timePassed = Time.time - startTimer;
        if (timePassed <= textActiveTime)
        {
            //Vector3 lookPos = new Vector3(GameManager.S.cameraRef.transform.position.x, GameManager.S.cameraRef.transform.position.y, GameManager.S.cameraRef.transform.position.z);
            //transform.LookAt(lookPos);
            //transform.Rotate(new Vector3(0, 180));
            transform.rotation = GameManager.S.cameraRef.transform.rotation;
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            transform.position += GameManager.S.cameraRef.transform.forward * moveSpeed * Time.deltaTime;
            textColor = GetComponent<MeshRenderer>().material.color;
            textColor.a += (-1*(timePassed / textActiveTime)) * Time.deltaTime;
            textColor = GetComponent<MeshRenderer>().material.color = textColor;
            //transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
            textActiveCount++;
            //alpha -= 0.4f;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator FadeText()
    {
        while (true)
        {
            if (textActiveCount <= textActiveTime)
            {
                //Vector3 lookPos = new Vector3(GameManager.S.cameraRef.transform.position.x, GameManager.S.cameraRef.transform.position.y, GameManager.S.cameraRef.transform.position.z);
                //transform.LookAt(lookPos);
                //transform.Rotate(new Vector3(0, 180));
                transform.rotation = GameManager.S.cameraRef.transform.rotation;
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                transform.position += GameManager.S.cameraRef.transform.forward * moveSpeed * Time.deltaTime;
                textColor = GetComponent<MeshRenderer>().material.color;
                textColor.a += -.8f * Time.deltaTime;
                textColor = GetComponent<MeshRenderer>().material.color = textColor;
                //transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
                textActiveCount++;
                //alpha -= 0.4f;
            }
            else
            {
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }
}
