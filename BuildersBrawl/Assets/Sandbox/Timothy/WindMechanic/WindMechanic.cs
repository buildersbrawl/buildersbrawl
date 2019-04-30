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
    public float windStartTimer = 1f;
    public float windDurationTImer = 1f;
    public float windSpeed = 1f;
    public GameObject[] players;

    private void Start()
    {
        StartCoroutine("WindTimer");
    }

    private void Update()
    {
        if (windInProgress)
        {
            Vector3 windFlow = GetWindFlowDirectiond(windDirection);

            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerMovement>().SetEnvironmentMomentum(windFlow);
            }

            
            for (int index = 0; index < GameManager.S.planksInScene.Count; index++)
            {
                //if plank is dropped
                if(GameManager.S.planksInScene[index].plankState == PlankManager.PlankState.dropped)
                {
                    //move it wiht wind
                    GameManager.S.planksInScene[index].GetComponent<Rigidbody>().AddForce(windFlow * 10f);
                }
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

    private IEnumerator WindTimer()
    {
        while (true)
        {
            if (windInProgress)
            {
                yield return new WaitForSeconds(windDurationTImer);
                windInProgress = false;
            }
            else
            {
                yield return new WaitForSeconds(windStartTimer);
                windInProgress = true;
            }
            yield return null;
        }
    }
}
