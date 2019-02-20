using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMechanic : MonoBehaviour
{
    public enum WindDirection
    {
        North,
        East,
        South,
        West
    }

    public WindDirection windDirection;
    public bool windInProgress = false;
    public float windSpeed;
    public GameObject player;

    private void Update()
    {
        if (windInProgress)
        {
            switch (windDirection)
            {
                case WindDirection.North:
                    player.transform.position += Vector3.forward * windSpeed * Time.deltaTime;
                    break;
                case WindDirection.East:
                    player.transform.position += Vector3.right * windSpeed * Time.deltaTime;
                    break;
                case WindDirection.South:
                    player.transform.position += Vector3.back * windSpeed * Time.deltaTime;
                    break;
                case WindDirection.West:
                    player.transform.position += Vector3.left * windSpeed * Time.deltaTime;
                    break;
                default:
                    Debug.Log("This direction does not exist!");
                    break;
            }
        }
    }

}
