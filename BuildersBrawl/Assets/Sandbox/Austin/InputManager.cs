using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputManager : MonoBehaviour
{
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

    void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
        player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);

    }

    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        
        //input for left stick
        moveVector.x = player.GetAxis("Move Horizontal");
        moveVector.z = player.GetAxis("Move Vertical");
        moveVector.y = 0f;
        if (moveVector != Vector3.zero)
            isMoving = true;

        //input for A, B, X, Y
        isJumping = player.GetButtonDown("Jump");
        isCharging = player.GetButtonDown("Charge");
        isPushing = player.GetButtonDown("Push");
        isUsingBoard = player.GetButtonDown("Use Board");

        //input for bumpbers and triggers
        isSlamming = player.GetButtonDown("Board Slam");
        if (player.GetAxis("Board Slam") != 0)
            isSlamming = true;

    }

    private void ProcessInput()
    {
        
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
}
