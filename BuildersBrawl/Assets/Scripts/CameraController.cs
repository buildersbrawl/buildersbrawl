﻿using System.Collections;
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
    GameObject player3ref;
    [SerializeField]
    GameObject player4ref;

    [SerializeField]
    GameObject cameraRef;

    [SerializeField]
    Vector3 averagePositionBetweenPlayers;
    [SerializeField]
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

    //turns off the camera pan/zoom controls when a player dies to avoid camera jerk
    public static bool playerDied = false; //use setCameraBasedOnPlayers instead
    //start and end markers when the player dies
    public Transform startMarker;
    public Transform endMarker;

    //speed at which the camera will move from startMarker to endMarker
    private float deathLerpTime = 5f;
    private bool shouldGetLerpStartTime = true;
    public float timeStartedLerping = 0f;
    public float deathTimer = 0f;

    public bool winnerDetermined = false;
    public GameObject winner;   //storage of winner GO
    public Vector3 winOffset = new Vector3(-10, 5, 0);   //position of the camera offset from the player
    public float winCameraHeight = 5f;
    private bool triggerWinRotate = false;
    public int winRotateSpeed = 20;
    private bool finalCameraPosition = false;
    private bool firstTime = false;
    private bool triggerPoints = true;

    private bool isStart = true;
    public GameObject startGameStart;
    public GameObject startGameEnd;
    public float startLerpRate = .005f;
    public bool isRightToLeft = false;
    public bool shouldDoStartPan = true;

    public AudioSource audio;
    public AudioClip[] clips;

    [SerializeField]
    private float deathStartDistance;
    [SerializeField]
    private float deathEndDistance;
    [SerializeField]
    private Vector3 deathStartAvgPos;
    [SerializeField]
    private Vector3 deathEndAvgPos;

    private List<float> distanceList;

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        Init();
        winnerDetermined = false;
        triggerWinRotate = false;
        Time.timeScale = 1f;
        
        //trigger the camera sweep before the game starts
        cameraRef.transform.position = startGameStart.transform.position;
        //set the audio source to the camera's audio source
        audio = cameraRef.GetComponent<AudioSource>();
        //set the first audio clip to be played as the start round noise
        //audio.clip = clips[0];
        
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

        if(player1ref == null || player2ref == null || player3ref == null || player4ref == null)
        {
            if(GameManager.S != null)
            {
                player1ref = GameManager.S.player1;
                player2ref = GameManager.S.player2;
                player3ref = GameManager.S.player3;
                player4ref = GameManager.S.player4;
            }
        }

        deathLerpTime = player1ref.GetComponent<PlayerDeath>().respawnTime * .8f;

        distanceList = new List<float>();

    }

    private void Update()
    {
        /*if (!shouldDoStartPan)
        {
            isStart = false;
        }

        if (isStart)
        {
            if (!isRightToLeft)
            {
                cameraRef.transform.LookAt(new Vector3(cameraRef.transform.position.x, startGameStart.transform.position.y, -20));
            }
            else
            {
                cameraRef.transform.LookAt(new Vector3(cameraRef.transform.position.x, startGameStart.transform.position.y, 20));
            }
            //cameraRef.transform.Rotate(new Vector3(0, -90, 0), Space.World);
            cameraRef.transform.position = Vector3.Lerp(cameraRef.transform.position, startGameEnd.transform.position, startLerpRate);
            startLerpRate = Mathf.Lerp(startLerpRate, .004f, .02f);
            Debug.Log("Start lerp rate = " + startLerpRate);
            //change isStart once it is close enough
            if (Vector3.Distance(cameraRef.transform.position, startGameEnd.transform.position) < 1f)
            {
                isStart = false;
                //play audio clip
                audio.Play(0);
                //change audio clip to the end of game sound
                audio.clip = clips[1];
            }
        }


        else */if (setCameraBasedOnPlayers && !winnerDetermined)
        {
            GetAveragePositionBetweenPlayers();
            GetDistanceBetweenPlayers();

            //reset bool for death lerping
            shouldGetLerpStartTime = true;

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
        else if (!winnerDetermined)
        {

            //Debug.Log("CameraController:Update");

            //move camera until it is even with position between players
            if (deathTimer < 5)
            {
                //increment the deathTimer
                deathTimer += Time.deltaTime;
                //Debug.Log("deathTimer = " + deathTimer);

                if (shouldGetLerpStartTime)
                {
                    timeStartedLerping = Time.time;
                    shouldGetLerpStartTime = false;
                }

                //Debug.Log("Is lerping after death");

                float timeSinceStarted = Time.time - timeStartedLerping;
                float percentageComplete = deathTimer / deathLerpTime;
                //Debug.Log("Percentage complete = " + percentageComplete);


                //--------------
               /* int playersDead = 0;
                foreach (PlayerController player in GameManager.S.playerList)
                {
                    if (player.playerDeath.playerDead)
                    {
                        playersDead++;
                    }
                }
                if (playersDead > 1)
                {
                    
                }*/

                SetDeathEndValues();

                //function for before and after teleport

                averagePositionBetweenPlayers = Vector3.Lerp(deathStartAvgPos, deathEndAvgPos, percentageComplete);
                distanceBetweenPlayers = Mathf.Lerp(deathStartDistance, deathEndDistance, percentageComplete);

                //---------------


                pitchPercent = (distanceBetweenPlayers - cameraPlayerDistanceFloor) / (cameraPlayerDistanceCeiling - cameraPlayerDistanceFloor);
                //cameraZOffset = -6.5f;
                
                //set the final camera position to the right spot
                SetCameraPosition(averagePositionBetweenPlayers);

                //lerp from the current position to the expected camera position once player spawns
                //cameraRef.transform.position = Vector3.Lerp(transform.position, cameraFinalPosition, percentageComplete);

                //set the FOV to set camera size
                SetCameraFOV(distanceBetweenPlayers);
                //set the camera height and pitch
                //averagePositionBetweenPlayers += new Vector3(0, -.005f, 0);
                AdjustCameraPitchAndHeightNew(distanceBetweenPlayers, averagePositionBetweenPlayers);

            }
            else
            {
                //turn on the camera settings with both players alive
                setCameraBasedOnPlayers = true;
                //reset death timer
                deathTimer = 0f;
                //reset cameraZOffset to original value
                //cameraZOffset = 0f;
            }
        }
        //if a winner has been determined, do stuff
        //delete comments after build
        /*else if (winnerDetermined)
        {
            ZoomOnWinner();
            //Debug.Log("We have a winner!!");
        }*/
    }

    public void SetCameraPosition(Vector3 centerPoint)
    {
        //+5x -5z

        //make sure y always = 10
        cameraFinalPosition = centerPoint;

        cameraFinalPosition.y += cameraYOffset;
        cameraFinalPosition.x += cameraXOffset;
        cameraFinalPosition.z += cameraZOffset;

        
        //dont transform if a player has died
        //if(setCameraBasedOnPlayers)
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
        //if(setCameraBasedOnPlayers)
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
        //distanceBetweenPlayers = 0;

        distanceList = new List<float>();

        if (player1ref != null && player2ref != null)
        {
            //gets float of distance between players
            distanceBetweenPlayers = Vector3.Distance(player1ref.transform.position, player2ref.transform.position);
            distanceList.Add(distanceBetweenPlayers);
        }
        else
        {
            print("Error: missing player reference");
        }

        if (player3ref != null)
        {
            float p1p2Dist = distanceBetweenPlayers;
            float p1p3Dist = Vector3.Distance(player1ref.transform.position, player3ref.transform.position);
            float p2p3Dist = Vector3.Distance(player2ref.transform.position, player3ref.transform.position);

            distanceList.Add(p1p2Dist);
            distanceList.Add(p1p3Dist);
            distanceList.Add(p2p3Dist);

            if (player4ref != null)
            {
                float p1p4Dist = Vector3.Distance(player1ref.transform.position, player4ref.transform.position);
                float p2p4Dist = Vector3.Distance(player2ref.transform.position, player4ref.transform.position);
                float p3p4Dist = Vector3.Distance(player3ref.transform.position, player4ref.transform.position);

                distanceList.Add(p1p4Dist);
                distanceList.Add(p2p4Dist);
                distanceList.Add(p3p4Dist);
            }
        }

        

        //distanceBetweenPlayers = distanceList[0];

        for (int index = 0; index < distanceList.Count; index++)
        {
            if(distanceBetweenPlayers < distanceList[index])
            {
                distanceBetweenPlayers = distanceList[index];
            }
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
            print("Error: missing player reference");
        }

        if (player3ref != null)
        {
            Vector3 p1p2 = averagePositionBetweenPlayers;
            Vector3 p1p3 = Vector3.Lerp(player1ref.transform.position, player3ref.transform.position, 0.5f);
            Vector3 p2p3 = Vector3.Lerp(player2ref.transform.position, player3ref.transform.position, 0.5f);

            Vector3 avg_p1p2_p1p3 = Vector3.Lerp(p1p2, p1p3, 0.5f);
            averagePositionBetweenPlayers = Vector3.Lerp(avg_p1p2_p1p3, p2p3, 0.5f);

            if (player4ref != null)
            {
                Vector3 p1p4 = Vector3.Lerp(player1ref.transform.position, player4ref.transform.position, 0.5f);
                Vector3 p2p4 = Vector3.Lerp(player2ref.transform.position, player4ref.transform.position, 0.5f);
                Vector3 p3p4 = Vector3.Lerp(player3ref.transform.position, player4ref.transform.position, 0.5f);

                Vector3 avg_p2p3_p1p4 = Vector3.Lerp(p2p3, p1p4, 0.5f);
                Vector3 avg_p2p4_p3p4 = Vector3.Lerp(p2p4, p3p4, 0.5f);

                Vector3 avg_p1p2_p1p3_p2p3_p1p4 = Vector3.Lerp(avg_p1p2_p1p3, avg_p2p3_p1p4, 0.5f);
                averagePositionBetweenPlayers = Vector3.Lerp(avg_p1p2_p1p3_p2p3_p1p4, avg_p2p4_p3p4, 0.5f);

            }
        }

        

    }

    public void SetDeathStartValues()
    {
        deathTimer = 0;

        //GetAveragePositionBetweenPlayers();
        //GetDistanceBetweenPlayers();

        /*int playersDead = 0;
        foreach (PlayerController player in GameManager.S.playerList)
        {
            print("dead chagne");
            if (player.playerDeath.playerDead)
            {
                playersDead++;
            }
        }
        if (playersDead <= 1)
        {
            
            
        }*/
        deathStartDistance = distanceBetweenPlayers;
        deathStartAvgPos = averagePositionBetweenPlayers;
    }

    public void SetDeathEndValues()
    {
        GetAveragePositionBetweenPlayers();
        GetDistanceBetweenPlayers();

        deathEndDistance = distanceBetweenPlayers;
        deathEndAvgPos = averagePositionBetweenPlayers;
    }

    private void ZoomOnWinner()
    {
        Vector3 newPos = new Vector3(GameObject.Find("Goal").transform.position.x, GameManager.S.winner.transform.position.y, GameObject.Find("Goal").transform.position.z);
        GameManager.S.winner.transform.position = newPos;

        Time.timeScale = 0.5f;
        //Debug.Log("Timescale = " + Time.timeScale);

        //turn controls off
        Vector3 winPos = GameManager.S.winner.transform.position + winOffset;
        
        if (Vector3.Distance(cameraRef.transform.position, winPos) <= .6f)
            triggerWinRotate = true;
        if (!triggerWinRotate)
        {
            //zoom
            //lerp from current position to position in front of player
            
            cameraRef.transform.position = Vector3.Lerp(cameraRef.transform.position, winPos, .05f);
            //set the camera height to a fixed value and pitch to lookat the winners position
            //AdjustCameraPitchAndHeightNew(winPos.y, GameManager.S.winner.transform.position);
            cameraRef.transform.LookAt(GameManager.S.winner.transform.position);    //adjust pitch
        }
        else if (triggerWinRotate)
        {
            if (triggerPoints)
            {
                GameManager.S.winner.GetComponent<FlashyPoints>().ShowPointsGained(GameManager.S.winner.gameObject.transform.position, GameManager.S.winner.gameObject.GetComponent<Points>().pointsForOtherSide);
                GameManager.S.winner.gameObject.GetComponent<Points>().AddPointsForOtherSide();
                triggerPoints = false;
            }

            //Debug.Log("CAMERA IN RIGHT POSITION");
            Debug.Log(cameraRef.transform.rotation.y);
            if (cameraRef.transform.rotation.y >= -.66)
            {
                //stop rotating camera
                finalCameraPosition = true;
                //Debug.Log("finalCameraPosition CAMERA POSITION IS TRUE");
            }
                
            if(!finalCameraPosition)
            {
                cameraRef.transform.RotateAround(GameManager.S.winner.transform.position, Vector3.up, -1 * winRotateSpeed * Time.deltaTime);
            }
            

            
        }
        //initiate slow motion
        //show flashy point ping 
        //move the points bar face by adding points here
    }
}

///Other optioons:
///Camera shake
///Make it so that interpolates (smoother transition)
///Make it so that camera zoom in/out once hit certain threshhold but also has a bit of zoom in/out variation when zoomed in/out