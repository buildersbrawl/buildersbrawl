using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoMechanic : MonoBehaviour
{
    public float timeUntilErupt;
    public float timeUntilExplosion;
    public float moveSpeed; //Lava Movement Speed
    public float lavaEndYPos; //Position to Halt Lava Movement
    public bool eruptCooldown = true;

    private Vector3 lavaStartingPos;
    private float startTimer;
    private Transform cameraTransform;


    private void Start()
    {
        lavaStartingPos = transform.position;
        startTimer = Time.time;
        cameraTransform = GameManager.S.cameraRef.transform;
    }

    private void Update()
    {
        float timePassed = Time.time - startTimer;

        if (!eruptCooldown)
        {
            Debug.Log("Eruption In Progress!");
            //StartCoroutine(ScreenShake(0.15f, 0.4f));
            if (transform.localPosition.y <= lavaEndYPos)
            {
                Debug.Log("Lava is Emerging!");
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;    
            }

        }
        else
        {
            Debug.Log("The Volcano Lies Dormant.");
            if (timePassed >= timeUntilErupt)
            {
                eruptCooldown = false; //End the Cooldown (Volcano is now active!)
                startTimer = Time.time;
            }
        }
    }

    private IEnumerator ScreenShake(float duration, float magnitude)
    {
        yield return new WaitForSeconds(timeUntilExplosion);
        Debug.Log("Shaking Screen");
        Vector3 originalPos = cameraTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraTransform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        cameraTransform.position = originalPos;
    }
}
