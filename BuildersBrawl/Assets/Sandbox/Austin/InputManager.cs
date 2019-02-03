using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float triggerSensitivity;

    public int playerID;
    private Player player;

    private Controller activeController;

    //direction player is moving
    public Vector3 moveVector;

    public bool isMoving = false;
    public bool isJumping = false;
    public bool isCharging = false;
    public bool isPushing = false;
    public bool isUsingBoard = false;
    public bool isSlamming = false;

    private Scene currentScene;
    private string sceneName = "";
    public bool controllerSelected = false;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    void Update()
    {
        GetInput();
        ProcessInput();
    }

    void GetInput()
    {
        //give player a playerID
        player = ReInput.players.GetPlayer(playerID);
        player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);

        if (sceneName == "ControllerSelectScreen" && controllerSelected == false)
        {
            controllerSelected = player.GetButtonDown("Submit");
        }
        
        
        if(sceneName != "ControllerSelectScreen")
        {
            //input for left stick
            moveVector.x = player.GetAxis("Move Horizontal");
            moveVector.z = player.GetAxis("Move Vertical");
            moveVector.y = 0f;
            if (moveVector != Vector3.zero)
                isMoving = true;
        }
        

        //input for A, B, X, Y
        isJumping = player.GetButtonDown("Jump");
        isCharging = player.GetButtonDown("Charge");
        isPushing = player.GetButtonDown("Push");
        isUsingBoard = player.GetButtonDown("Use Board");

        //input for bumpbers
        isSlamming = player.GetButtonDown("Board Slam");
        
        //if trigger is pressed beyond triggerSensitivity it will trigger the slam
        if (player.GetAxis("Board Slam") >= triggerSensitivity)
            isSlamming = true;

    }

    void ProcessInput()
    {

        //if(controllerSelected)


        if (isMoving)
            Debug.Log("Moving " + moveVector.x + " in the X direction and " + moveVector.z + " in the Z direction");

        if (isJumping)
            Debug.Log("Jump");

        if (isCharging)
            Debug.Log("Charge");

        if (isPushing)
            Debug.Log("Push");

        if (isUsingBoard)
            Debug.Log("Using Board");

        if (isSlamming)
            Debug.Log("Slam");
    }

    public void SelectController1()
    {
        playerID = 1;
        Debug.Log("ID = 1");
    }

    public void SelectedController2()
    {
        playerID = 2;
        Debug.Log("ID = 2");
    }
}
