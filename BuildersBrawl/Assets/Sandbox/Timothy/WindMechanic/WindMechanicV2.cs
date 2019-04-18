﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMechanicV2 : MonoBehaviour
{
    public enum WindDirection
    {
        North,
        East,
        South,
        West
    }

    public WindDirection windDirection;
    public float windSpeed = 1f;
    public float timeUntilWind;
    public float windDuration;
    public bool windCooldown = true;

    private PlayerController[] playerLocator;
    private GameObject[] players;
    private float startTimer;
    private Vector3 windFlow;
    private Vector3 windDir;

    [Header("Always set to 1")]
    [Tooltip("Used to see how this percentage is interacting")]
    public float windPercentage = 1;
    public float windReductionStartDistance;
    public float windReductionMaxDistance;
    LayerMask mask;

    private void Awake()
    {
        playerLocator = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
        players = new GameObject[playerLocator.Length];

        mask = LayerMask.GetMask("Default");
    }

    private void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = playerLocator[i].gameObject;
        }
        startTimer = Time.time;
        windFlow = GetWindFlowDirectiond(windDirection);

    }

    private void Update()
    {
        float timePassed = Time.time - startTimer;
        if (!windCooldown)
        {
            Debug.Log("The Wind is aBlowing!");
            if (timePassed >= windDuration)
            {
                windCooldown = true;
                startTimer = Time.time;
            }
            for (int i = 0; i < players.Length; i++)
            {
                Debug.DrawRay(players[i].transform.position, -windDir * windReductionStartDistance, Color.red);
                if (!Physics.Raycast(players[i].transform.position, -windDir, out RaycastHit hit, windReductionStartDistance, mask))
                {
                    
                    players[i].GetComponent<PlayerMovement>().SetEnvironmentMomentum(windFlow);
                }
                else
                {
                    if (hit.distance > windReductionMaxDistance && hit.distance <= windReductionStartDistance)
                    {
                        windPercentage = (hit.distance - windReductionMaxDistance) / (windReductionStartDistance - windReductionMaxDistance);
                        players[i].GetComponent<PlayerMovement>().SetEnvironmentMomentum(windFlow * windPercentage);
                    }
                }
                //players[i].GetComponent<PlayerMovement>().SetEnvironmentMomentum(windFlow);

            }
        }
        else
        {
            Debug.Log("The Land Remains Silent!");
            if (timePassed >= timeUntilWind)
            {
                windCooldown = false;
                startTimer = Time.time;
            }
        }
    }

    private Vector3 GetWindFlowDirectiond(WindDirection direction)
    {
        Vector3 windDirection = Vector3.zero;

        switch (direction)
        {
            case WindDirection.North:
                windDir = Vector3.forward;
                windDirection = Vector3.forward * Time.deltaTime * windSpeed;
                break;
            case WindDirection.East:
                windDir = Vector3.right;
                windDirection = Vector3.right * Time.deltaTime * windSpeed;
                break;
            case WindDirection.South:
                windDir = Vector3.back;
                windDirection = Vector3.back * Time.deltaTime * windSpeed;
                break;
            case WindDirection.West:
                windDir = Vector3.left;
                windDirection = Vector3.left * Time.deltaTime * windSpeed;
                break;
            default:
                Debug.Log("This direction does not exist!");
                break;
        }
        return windDirection;
    }
}
