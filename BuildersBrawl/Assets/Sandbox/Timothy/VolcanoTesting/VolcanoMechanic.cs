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
    public float shakeDuration;
    public float shakeMagnitude;

    private Vector3 lavaStartingPos;
    private float startTimer;
    private Transform cameraTransform;


    private void Start()
    {
        lavaStartingPos = transform.position;
        startTimer = Time.time;
        cameraTransform = GameManager.S.cameraRef.transform;
        GameManager.S.cameraRef.GetComponent<CameraController>().volcRef = this;
    }

    private void Update()
    {
        float timePassed = Time.time - startTimer;

        if (!eruptCooldown)
        {
            if (timePassed >= timeUntilErupt)
            {
                GameManager.S.cameraRef.GetComponent<CameraController>().cameraShake = true;
            }
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

    public IEnumerator ScreenShake()
    {
        float elapsed = 0.0f;

        Vector3 originalCamPos = cameraTransform.position;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / shakeDuration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;

            x *= shakeMagnitude * damper;
            y *= shakeMagnitude * damper;

            cameraTransform.position = new Vector3(x, y, originalCamPos.z);

            yield return null;
        }
        cameraTransform.position = originalCamPos;
    }
    /*
    public IEnumerator ScreenShake()
    {
        yield return new WaitForSeconds(timeUntilExplosion);
        Debug.Log("Shaking Screen");
        //Vector3 originalPos = cameraTransform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float z = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.position = new Vector3(x, cameraTransform.position.y, z);
            Debug.Log("Elapsed Time: " + elapsed);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        //cameraTransform.position = originalPos;
    }*/
}
