using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameInputManager : MonoBehaviour
{
    public int playerID;
    private Player player;

    private Controller activeController;

    //direction player is moving
    public Vector3 joystickInput; //joystick

    public bool isMoving = false;
    public bool pressedJumpButton = false; //A
    public bool pressedChargeButton = false; //B
    public bool pressedPushButton = false; //X
    public bool pressedBoardPickUpOrDropButton = false; //Y
    public bool pressedSlamButton = false; //bunmpers and triggers

    void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
        player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);

    }

    void Update()
    {
        GetInput();
        //ProcessInput();
    }

    private void GetInput()
    {

        //input for left stick
        joystickInput.x = player.GetAxis("Move Horizontal");
        joystickInput.z = player.GetAxis("Move Vertical");
        joystickInput.y = 0f;
        if (joystickInput != Vector3.zero)
            isMoving = true;

        //input for A, B, X, Y
        pressedJumpButton = player.GetButtonDown("Jump");
        pressedChargeButton = player.GetButtonDown("Charge");
        pressedPushButton = player.GetButtonDown("Push");
        pressedBoardPickUpOrDropButton = player.GetButtonDown("Use Board");

        //input for bumpbers and triggers
        pressedSlamButton = player.GetButtonDown("Board Slam");
        if (player.GetAxis("Board Slam") != 0)
            pressedSlamButton = true;

    }

    private void ProcessInput()
    {

        if (isMoving)
            Debug.Log("Moving " + joystickInput.x + " in the X direction and " + joystickInput.z + " in the Z direction");

        if (pressedJumpButton)
            Debug.Log("Jump");

        if (pressedChargeButton)
            Debug.Log("Charge");

        if (pressedPushButton)
            Debug.Log("Push");

        if (pressedBoardPickUpOrDropButton)
            Debug.Log("Using Board");

        if (pressedSlamButton)
            Debug.Log("Slam");
    }
}
