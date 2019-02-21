using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraOptions
    {
        option1,
        option2,
        option3,
        side,
        front,
        fortyFiveDegrees
    }
    //gets average point between players for camera position
    //gets size of distance between players for camera size

        /// <summary>
        /// Camera pan backwar and side to side
        /// Camera zooms/pans back and moves down/angles up based off of player position
        /// 
        /// </summary>

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    [Header("References")]
    [SerializeField]
    GameObject player1ref;
    [SerializeField]
    GameObject player2ref;

    [SerializeField]
    GameObject cameraRef;

    [SerializeField]
    Vector3 averagePositionBetweenPlayers;
    float distanceBetweenPlayers;

    [Header("Options")]
    [Tooltip("Opt1: Camera zooms in and out based off of player distance " +
        "/nOpt2: Camera zooms in a certain amount or zooms out a certain amount based off of whether or not the distance between players reaches a threshold" +
        "/nOpt3: Combo of Opt1 & Opt2")]
    public CameraOptions cameraOptions;

    //OBSELETE
    //[Header("Option 1")]
    [Tooltip("The larger the number the more extreme the zoom in/out")]
    //[SerializeField]
    private float cameraSizeFactorOpt1 = 1f;            //simply used to increase/decrease how large the base camera is
    //[Header("Option 2")]
    //[SerializeField]
    private float howClosePlayersNeedToBeForZoomOpt2 = 2f;
    //[Header("Option 3")]
    //[SerializeField]
    private float cameraZoomVariationLimitCloseUp = 2f;
    //[SerializeField]
    private float cameraZoomVariationLimitOut = 5f;
    //[SerializeField]
    //[Tooltip("The larger the number the more extreme the zoom in/out")]
    private float cameraSizeFactorOpt3 = .1f;

    Vector3 cameraFinalPosition;
    float cameraFinalSize;

    [Header("Position")]
    [Tooltip("Offset of camera position on vertical axis")]
    [SerializeField]
    private float cameraXOffset = 6.5f;
    [SerializeField]
    private float cameraZOffset = 6.5f;
    [SerializeField]
    private float cameraYOffset = 3f;

    //OBSELETYE
    //[Header("Size")]
    [Tooltip("Opt1: How zoomed in the camera can be \nOpt2: Amount camera zooms in /nOpt3: What size camera uses as reference point for being close up")]
    //[SerializeField]
    private float cameraSizeFloor = 1f;
    [Tooltip("Opt1: How zoomed out the camera can be \nOpt2: Amount camera zooms out /nOpt3: What size camera uses as reference point for being far away")]
    //[SerializeField]
    private float cameraSizeCeiling = 20f;
    [SerializeField]
    private float cameraZoomSpeed = .1f;

    [Header("Perspective")]
    [SerializeField]
    private float cameraFOVFactor = 10f;            //simply used to increase/decrease how large the base camera is
    [Tooltip("How zoomed in the camera can be")]
    [SerializeField]
    private float cameraFOVFloor = 25f;
    [Tooltip("How zoomed out the camera can be")]
    [SerializeField]
    private float cameraFOVCeiling = 50f;
    private float cameraFinalFOV;

    [Header("Adjust Pitch and Height")]
    //[Tooltip("How low an angle the camera can be")]
    //[SerializeField]
    //float cameraAngleFloor = 5;
    //[Tooltip("How high an angle the camera can be")]
    //[SerializeField]
    //float cameraAngleCeiling = 80;
    [Tooltip("How low the camera can be")]
    [SerializeField]
    float cameraYHeightFloor = 5;           
    [Tooltip("How high the camera can be")]
    [SerializeField]
    float cameraYHeightCeiling = 27;
    //[Tooltip("How close the camera can be")]
    //[SerializeField]
    //float cameraAdjacentAxisFloor = 8;
    //[Tooltip("How far the camera can be")]
    //[SerializeField]
    //float cameraAdjacentAxisCeiling = 12;
    [Header("Percentage")]
    [Tooltip("How close the players must be to hit bottom angle")]
    [SerializeField]
    float cameraPlayerDistanceFloor = 3;           
    [Tooltip("How far the players must be to hit top angle")]
    [SerializeField]
    float cameraPlayerDistanceCeiling = 20;
    float pitchPercent; //45 degrees is ~.533333
    private float startingYAngle;
    private float finalPitchAngle;
    private float finalCameraHeight;
    //public float finalCameraAA;
    private Vector3 cameraRotationSetter;
    private Vector3 cameraHeightSetter;
    //private Vector3 cameraAASetter;

    [Header("Other")]
    [Tooltip("Whether or not the camera is setting its position/size based off of the players")]
    public bool setCameraBasedOnPlayers = true;

    [Tooltip("Players cannot go further apart than this")]
    //[SerializeField]
    private float playersApartMaxDistance = 50f;

    //turns off the camera pan/zoom controls when a player dies to avoid camera jerk
    public static bool playerDied = false; //use setCameraBasedOnPlayers instead
    //start and end markers when the player dies
    public Transform startMarker;
    public Transform endMarker;
    //speed at which the camera will move from startMarker to endMarker
    public float deathCameraMoveSpeed = 1.0f;

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        Init();
        
        //set the end marker to the initial camera position
        endMarker = cameraRef.transform;
    }

    public void Init()
    {
        if (cameraRef == null)
        {
            cameraRef = this.gameObject;
        }

        if (cameraOptions == CameraOptions.side)
        {
            //SHOULD GO IN INIT
            //y angle = -90 or 270
            startingYAngle = 270;
            //apply
            cameraRotationSetter = cameraRef.transform.eulerAngles;
            cameraRotationSetter.y = startingYAngle;

            //x angle starts off as 45
            //y pso offset starts at 10
            cameraYOffset = 10;
            //z pos offset = 0
            cameraZOffset = 0;
        }

        else if (cameraOptions == CameraOptions.front)
        {
            //SHOULD GO IN INIT
            //y angle = -90 or 270
            startingYAngle = 180;
            cameraRotationSetter = cameraRef.transform.eulerAngles;
            cameraRotationSetter.y = startingYAngle;
            //x angle starts off as 45
            //y pso offset starts at 10
            cameraYOffset = 10;
            //z pos offset = 0
            cameraXOffset = 0;
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
                //OBSELETE
                print("OBSELETE");
                //apply distance between players to set camera size algorhythm
                //SetCameraSizeOpt1(distanceBetweenPlayers);

            }
            else if (cameraOptions == CameraOptions.option2)
            {
                //OBSELETE
                print("OBSELETE");
                //apply distance between players to set camera size algorhythm
                //SetCameraSizeOpt2(distanceBetweenPlayers);
            }
            else if (cameraOptions == CameraOptions.option3)
            {
                //OBSELETE
                print("OBSELETE");
                //apply distance between players to set camera size algorhythm
                //SetCameraSizeOpt3(distanceBetweenPlayers);
            }
            else if (cameraOptions == CameraOptions.side)
            {

                pitchPercent = (distanceBetweenPlayers - cameraPlayerDistanceFloor) / (cameraPlayerDistanceCeiling - cameraPlayerDistanceFloor);

                SetCameraPosition(averagePositionBetweenPlayers);

                //apply distance between players to set camera size algorhythm
                SetCameraFOV(distanceBetweenPlayers);

                //add height and pitch(up and down angle) adjustment
                AdjustCameraPitchAndHeightNew(distanceBetweenPlayers, averagePositionBetweenPlayers);
                //print(cameraYOffset);
            }
            else if (cameraOptions == CameraOptions.front)
            {

                pitchPercent = (distanceBetweenPlayers - cameraPlayerDistanceFloor) / (cameraPlayerDistanceCeiling - cameraPlayerDistanceFloor);

                SetCameraPosition(averagePositionBetweenPlayers);

                //apply distance between players to set camera size algorhythm
                SetCameraFOV(distanceBetweenPlayers);
                //add height and pitch(up and down angle) adjustment
                AdjustCameraPitchAndHeightNew(distanceBetweenPlayers, averagePositionBetweenPlayers);


            }
            else
            {
                print("Error: no option for this cameraSize option");
            }
        }
        //when a player dies, use these camera movement serttings 
        else
        {
            //set the startMarker to the current transform only during the initial frame it is run
            bool hasStartMarker = false;
            if(!hasStartMarker)
            {
                startMarker = transform;
                hasStartMarker = true;
            }

            //length of the total journey the camera will move
            float journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
            float startTime = Time.time;
            float distCovered = (Time.time - startTime) * deathCameraMoveSpeed;
            //a fraction of the total journey covered during this frame
            float fractionJourney = distCovered / journeyLength;

            //lerp or move the camera
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionJourney);

            //if the camera has reached the destination turn on normal cameras settings
            if(Vector3.Distance(transform.position, endMarker.position) < 0.2f)
            {
                setCameraBasedOnPlayers = true;
            }
        }
    }

    public void SetCameraPosition(Vector3 centerPoint)
    {
        //+5x -5z

        //make sure y always = 10
        cameraFinalPosition = centerPoint;

        cameraFinalPosition.y += cameraYOffset;
        cameraFinalPosition.x += cameraXOffset;
        cameraFinalPosition.z += cameraZOffset;


        cameraRef.transform.position = cameraFinalPosition;
    }

    public void AdjustCameraPitchAndHeightNew(float distanceBetweenPlayers, Vector3 averagePlayerPosition)
    {
        //larger distance = higher height, larger angle
        //if distance between players is less than 23 (cameraAnglePlayerDistanceCeiling) camera angle below 80 (cameraAngleCeiling)
        //smaller distance = lower height, smaller angle
        //if distance between players is greater than 3 (cameraAnglePlayerDistanceFloor) camera angle above 5 (cameraAngleFloor)

        //done earlier
        //pitchPercent = (distanceBetweenPlayers - cameraPlayerDistanceFloor) / (cameraPlayerDistanceCeiling - cameraPlayerDistanceFloor);

        //because using lerp will automatically stop camera from going above or below angle

        //finalPitchAngle = Mathf.Lerp(cameraAngleFloor, cameraAngleCeiling, pitchPercent);

        //height = yoffset + average position height
        cameraYOffset = Mathf.Lerp(cameraYHeightFloor + averagePlayerPosition.y, cameraYHeightCeiling + averagePlayerPosition.y, pitchPercent);

        //apply
        //height
        cameraHeightSetter = cameraRef.transform.position;
        cameraHeightSetter.y = cameraYOffset;
        cameraRef.transform.position = cameraHeightSetter;

        //angle
        cameraRef.transform.LookAt(averagePlayerPosition);

        
    }

    /*
    //OLD
        public void AdjustCameraPitchAndHeight(float distanceBetweenPlayers, Vector3 averagePlayerPosition)
    {
        //larger distance = higher height, larger angle
        //if distance between players is less than 23 (cameraAnglePlayerDistanceCeiling) camera angle below 80 (cameraAngleCeiling)
        //smaller distance = lower height, smaller angle
        //if distance between players is greater than 3 (cameraAnglePlayerDistanceFloor) camera angle above 5 (cameraAngleFloor)

        //done earlier
        //pitchPercent = (distanceBetweenPlayers - cameraPlayerDistanceFloor) / (cameraPlayerDistanceCeiling - cameraPlayerDistanceFloor);

        //because using lerp will automatically stop camera from going above or below angle

        finalPitchAngle = Mathf.Lerp(cameraAngleFloor, cameraAngleCeiling, pitchPercent);

        //height = yoffset + average position height
        cameraYOffset = Mathf.Lerp(cameraYHeightFloor, cameraYHeightCeiling, pitchPercent);

        //apply
        //angle
        cameraRotationSetter = cameraRef.transform.eulerAngles;
        cameraRotationSetter.x = finalPitchAngle;
        cameraRef.transform.eulerAngles = cameraRotationSetter;

        //height
        cameraHeightSetter = cameraRef.transform.position;
        cameraHeightSetter.y = cameraYOffset;
        cameraRef.transform.position = cameraHeightSetter;

        //adjacent axis should also be offset
        if(cameraOptions == CameraOptions.side)
        {
            cameraXOffset = Mathf.Lerp(cameraAdjacentAxisFloor, cameraAdjacentAxisCeiling, pitchPercent) + averagePlayerPosition.x;

            //aa = x
            cameraAASetter = cameraRef.transform.position;
            cameraAASetter.x = cameraXOffset;
            cameraRef.transform.position = cameraAASetter;

        }
        else if(cameraOptions == CameraOptions.front)
        {
            cameraZOffset = Mathf.Lerp(cameraAdjacentAxisFloor, cameraAdjacentAxisCeiling, pitchPercent) + averagePlayerPosition.z;

            //aa = z
            cameraAASetter = cameraRef.transform.position;
            cameraAASetter.z = cameraZOffset;
            cameraRef.transform.position = cameraAASetter;
        }
        else
        {
            print("Adjacent Axis not set up for this, will have no variation forward or backward");
        }

    }

*/
    //essentially the zoom in or out with floor/ceiling
    public void SetCameraFOV(float distanceBetweenPlayers)
    {
        //set size
        cameraFinalFOV = distanceBetweenPlayers * cameraFOVFactor;

        //stop camera from getting too big or small
        if (cameraFinalFOV >= cameraFOVCeiling)
        {
            cameraFinalFOV = cameraFOVCeiling;
        }
        else if (cameraFinalFOV <= cameraFOVFloor)
        {
            cameraFinalFOV = cameraFOVFloor;
        }

        cameraRef.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cameraRef.GetComponent<Camera>().fieldOfView, cameraFinalFOV, cameraZoomSpeed);

    }

    //OBSELETE
    /*
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
    */

    public void GetDistanceBetweenPlayers()
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

        //if distance too large stop players
        if (distanceBetweenPlayers > playersApartMaxDistance)
        {
            print("players too far apart. Note to self: figure out how to limit this");
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