using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashyPointsController : MonoBehaviour
{
    public int textActiveTime = 5;
    public float moveSpeed;
    private int textActiveCount = 0;

    private void Start()
    {
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        while (true)
        {
            if (textActiveCount <= textActiveTime)
            {
                Vector3 lookPos = new Vector3(GameManager.S.cameraRef.transform.position.x, GameManager.S.cameraRef.transform.position.y, GameManager.S.cameraRef.transform.position.z);
                transform.LookAt(lookPos);
                transform.Rotate(new Vector3(0, 180));
                //transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);
                textActiveCount++;
            }
            else
            {
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }
}
