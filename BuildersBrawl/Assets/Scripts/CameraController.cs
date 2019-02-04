using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum CameraOptions
    {
        option1,
        option2,
        option3
    }
    //gets average point between players for camera position
    //gets size of distance between players for camera size

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    [Header("References")]
    [SerializeField]
    GameObject player1ref;
    [SerializeField]
    GameObject player2ref;

    [SerializeField]
    GameObject cameraRef;

    Vector3 averagePositionBetweenPlayers;
    float distanceBetweenPlayers;

    [Header("Options")]
    [Tooltip("Opt1: Camera zooms in and out based off of player distance " +
        "/nOpt2: Camera zooms in a certain amount or zooms out a certain amount based off of whether or not the distance between players reaches a threshold" +
        "/nOpt3: Combo of Opt1 & Opt2")]
    [SerializeField]
    CameraOptions cameraOptions;
    [Header("Option 1")]
    [Tooltip("The larger the number the more extreme the zoom in/out")]
    [SerializeField]
    private float cameraSizeFactorOpt1 = 1f;            //simply used to increase/decrease how large the base camera is
    [Header("Option 2")]
    [SerializeField]
    private float howClosePlayersNeedToBeForZoomOpt2 = 2f;
    [Header("Option 3")]
    [SerializeField]
    private float cameraZoomVariationLimitCloseUp = 2f;
    [SerializeField]
    private float cameraZoomVariationLimitOut = 5f;
    [SerializeField]
    [Tooltip("The larger the number the more extreme the zoom in/out")]
    private float cameraSizeFactorOpt3 = .1f;

    Vector3 cameraFinalPosition;
    float cameraFinalSize;

    [Header("Position")]
    [Tooltip("Offset of camera position on vertical axis")]
    [SerializeField]
    private float cameraVertOffset = 6.5f;

    [Header("Size")]
    [Tooltip("Opt1: How zoomed in the camera can be \nOpt2: Amount camera zooms in /nOpt3: What size camera uses as reference point for being close up")]
    [SerializeField]
    private float cameraSizeFloor = 1f;
    [Tooltip("Opt1: How zoomed out the camera can be \nOpt2: Amount camera zooms out /nOpt3: What size camera uses as reference point for being far away")]
    [SerializeField]
    private float cameraSizeCeiling = 20f;
    [SerializeField]
    private float cameraZoomSpeed = .1f;

    [Header("Other")]
    [Tooltip("Whether or not the camera is setting its position/size based off of the players")]
    public bool setCameraBasedOnPlayers = true;

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if(cameraRef == null)
        {
            cameraRef = this.gameObject;
        }
    }

    private void Update()
    {
        if (setCameraBasedOnPlayers)
        {
            GetAveragePositionBetweenPlayers();
            GetDistanceBetweenPlayers();

            //apply average positon to camera position algorythm
            SetCameraPosition(averagePositionBetweenPlayers);

            if (cameraOptions == CameraOptions.option1)
            {
                //apply distance between players to set camera size algorhythm
                SetCameraSizeOpt1(distanceBetweenPlayers);
            }
            else if (cameraOptions == CameraOptions.option2)
            {
                //apply distance between players to set camera size algorhythm
                SetCameraSizeOpt2(distanceBetweenPlayers);
            }
            else if (cameraOptions == CameraOptions.option3)
            {
                //apply distance between players to set camera size algorhythm
                SetCameraSizeOpt3(distanceBetweenPlayers);
            }
            else
            {
                print("Error: no option for this cameraSize option");
            }
        }
    }

    public void SetCameraPosition(Vector3 centerPoint)
    {
        //+5x -5z

        //make sure y always = 10
        cameraFinalPosition = centerPoint;

        cameraFinalPosition.y += 10f;
        cameraFinalPosition.x += cameraVertOffset;
        cameraFinalPosition.z -= cameraVertOffset;


        cameraRef.transform.position = cameraFinalPosition;
    }

    //essentially the zoom in or out with floor/ceiling
    public void SetCameraSizeOpt1(float distanceBetweenPlayers)
    {
        //set size
        cameraFinalSize = distanceBetweenPlayers * cameraSizeFactorOpt1;

        //stop camera from getting too big or small
        if (cameraFinalSize >= cameraSizeCeiling)
        {
            cameraFinalSize = cameraSizeCeiling;
        }
        else if (cameraFinalSize <= cameraSizeFloor)
        {
            cameraFinalSize = cameraSizeFloor;
        }

        cameraRef.GetComponent<Camera>().orthographicSize = Mathf.Lerp(cameraRef.GetComponent<Camera>().orthographicSize, cameraFinalSize, cameraZoomSpeed);

    }

    //essentially the zoom in or out but dont do it unless hit a certain threshold
    public void SetCameraSizeOpt2(float distanceBetweenPlayers)
    {
        if(distanceBetweenPlayers > howClosePlayersNeedToBeForZoomOpt2)
        {
            //zoom out
            cameraFinalSize = cameraSizeCeiling;
        }
        else
        {
            //zoom in
            cameraFinalSize = cameraSizeFloor;
        }

        //interpolate to desired point
        cameraRef.GetComponent<Camera>().orthographicSize = Mathf.Lerp(cameraRef.GetComponent<Camera>().orthographicSize, cameraFinalSize, cameraZoomSpeed);

    }

    //OPT3

    //essentially the zoom in or out but does it to an extreme when hits certain threshold
    public void SetCameraSizeOpt3(float distanceBetweenPlayers)
    {
        if (distanceBetweenPlayers > howClosePlayersNeedToBeForZoomOpt2)
        {
            //zoom out
            cameraFinalSize = cameraSizeCeiling + (distanceBetweenPlayers * cameraSizeFactorOpt3);
            if (cameraFinalSize > cameraSizeCeiling + cameraZoomVariationLimitOut)
            {
                cameraFinalSize = cameraSizeCeiling + cameraZoomVariationLimitOut;
            }
            else if (cameraFinalSize < cameraSizeCeiling - cameraZoomVariationLimitOut)
            {
                cameraFinalSize = cameraSizeCeiling - cameraZoomVariationLimitOut;
            }

        }
        else
        {
            //zoom in
            cameraFinalSize = cameraSizeFloor + (distanceBetweenPlayers * cameraSizeFactorOpt3);
            if (cameraFinalSize > cameraSizeFloor + cameraZoomVariationLimitCloseUp)
            {
                cameraFinalSize = cameraSizeFloor + cameraZoomVariationLimitCloseUp;
            }
            else if (cameraFinalSize < cameraSizeFloor - cameraZoomVariationLimitCloseUp)
            {
                cameraFinalSize = cameraSizeFloor - cameraZoomVariationLimitCloseUp;
            }
        }

        //interpolate to desired point
        cameraRef.GetComponent<Camera>().orthographicSize = Mathf.Lerp(cameraRef.GetComponent<Camera>().orthographicSize, cameraFinalSize, cameraZoomSpeed);

    }

    private void GetDistanceBetweenPlayers()
    {
        if(player1ref != null && player2ref != null)
        {
            //gets float of distance between players
            distanceBetweenPlayers = Vector3.Distance(player1ref.transform.position, player2ref.transform.position);
        }
        else
        {
            print("Error: you need to drag and drop the player gameobjects into the CameraController");
        }
    }

    private void GetAveragePositionBetweenPlayers()
    {
        if (player1ref != null && player2ref != null)
        {
            //gets point in the center of these two
            averagePositionBetweenPlayers = Vector3.Lerp(player1ref.transform.position, player2ref.transform.position, 0.5f);
        }
        else
        {
            print("Error: you need to drag and drop the player gameobjects into the CameraController");
        }
    }

}

///Other optioons:
///Camera shake
///Make it so that interpolates (smoother transition)
///Make it so that camera zoom in/out once hit certain threshhold but also has a bit of zoom in/out variation when zoomed in/out