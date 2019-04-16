using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoMechanic : MonoBehaviour
{
    public float timeUntilErupt;
    public float timeUntilExplosion;
    public float moveSpeed; //Lava Movement Speed
    public Vector3 lavaEndPos; //Position to Halt Lava Movement
    public bool eruptCooldown = true;
    public float shakeDuration = 0f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector3 lavaStartingPos;
    private float startTimer;
    private Transform cameraTransform;
    private Vector3 camOrigPos;


    private void Start()
    {
        lavaStartingPos = transform.position;
        startTimer = Time.time;
        cameraTransform = GameManager.S.cameraRef.transform;
        camOrigPos = cameraTransform.localPosition;
    }

    private void Update()
    {
        float timePassed = Time.time - startTimer;

        if (!eruptCooldown)
        {
            Debug.Log("Eruption In Progress!");
            if (timePassed >= timeUntilExplosion)
            {
                //ScreenShake();
            }

            if (transform.position.y <= lavaEndPos.y)
            {
                transform.position = Vector3.up * moveSpeed * Time.deltaTime;    
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

    private void ScreenShake()
    {
        //Shake the Screen
        //https://gist.github.com/ftvs/5822103
        if (shakeDuration > 0)
        {
            cameraTransform.localPosition = camOrigPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            cameraTransform.localPosition = camOrigPos;
        }

    }
}
