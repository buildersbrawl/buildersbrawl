using System.Collections;
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

    private void Awake()
    {
        playerLocator = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
        players = new GameObject[playerLocator.Length];
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
        RaycastHit hit;
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
                if (Physics.Raycast(players[i].transform.position + (2.3f * windFlow), -windFlow, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.GetComponent<PlayerController>())
                    {
                        players[i].GetComponent<PlayerMovement>().SetEnvironmentMomentum(windFlow);
                    }
                    //Debug.Log("What was hit?: " + hit.collider.gameObject);
                    //Debug.DrawRay(players[i].transform.position + windFlow, -windFlow, Color.red);
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
                windDirection = Vector3.forward * Time.deltaTime * windSpeed;
                break;
            case WindDirection.East:
                windDirection = Vector3.right * Time.deltaTime * windSpeed;
                break;
            case WindDirection.South:
                windDirection = Vector3.back * Time.deltaTime * windSpeed;
                break;
            case WindDirection.West:
                windDirection = Vector3.left * Time.deltaTime * windSpeed;
                break;
            default:
                Debug.Log("This direction does not exist!");
                break;
        }
        return windDirection;
    }
}
