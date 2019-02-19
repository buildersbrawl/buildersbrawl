using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //player controller ref
    PlayerController playerController;

    public float playerSpeed = 1f;
    [Tooltip("What percentage the player slows down when jumping/holding a plank")]
    public float slowDownSpeedPercentage = .1f;

    Vector3 playerFinalDirection; 

    [Header("Jumping Variables")]
    [Tooltip("Starts when player returns to ground (OBSELETE)")]
    public float postJumpCooldown = .2f; //OBSELETE
    public float jumpHeight = 7f;
    public float holdingBoardJumpHeight = 3f;
    private Vector3 jumpVector;

    [Header("Physics")]
    public float gravity = -8f;
    private Vector3 gravityVector;

    private Vector3 playerMomentum;
    public Vector3 PlayerMomentum
    {
        get
        {
            return playerMomentum;
        }
        set
        {
            playerMomentum = value;
        }
    }

    [SerializeField]
    private float groundDrag = .1f;
    [SerializeField]
    private float airDrag = 0f;

    //can use to get player momentum
    private Vector3 reversePlayerMovementFromJoysticks;


    //-------------------------------------------------------------------------------------------------------------------------------------------------------------


    public void InitMove(PlayerController pC)
    {
        playerController = pC;
        jumpVector = new Vector3(0, jumpHeight, 0);
        gravityVector = new Vector3(0, gravity, 0);
    }

    public Vector3 PlayerSideMovement(Vector3 direction, PlayerController.PlayerState pState)
    {
        
        //if player jumping or holding plank slow them down
        if(playerController.playerState == PlayerController.PlayerState.jumping || playerController.playerState == PlayerController.PlayerState.holdingPlank)
        {
            //keep player momentum 
            playerFinalDirection = playerMomentum;

            //and add small amount of input direction
            playerFinalDirection += (direction * playerSpeed) * slowDownSpeedPercentage;
            reversePlayerMovementFromJoysticks = ((direction * playerSpeed) * slowDownSpeedPercentage) * -1;

            //TODO: figure out how to stop player from doing accelerated jump
            //clamp magnitude so players cannot accelerate past momentum
            //Mathf.Clamp(playerFinalDirection.magnitude, 0, playerMomentum.magnitude);
        }
        else
        {
            //keep player momentum 
            playerFinalDirection = playerMomentum;

            reversePlayerMovementFromJoysticks = (direction * playerSpeed) * -1;
            playerFinalDirection += direction * playerSpeed;

        }
            
        return playerFinalDirection;
    }

    public Vector3 Jump(bool buttonPressed, bool holdingPlank)
    {
        //if button pressed and not holding plank
        if (buttonPressed)
        {
            //print("Jump button pressed");

            //record player momentum
            playerMomentum = playerController.LastFramesMoveVector;
            print("Momentum is " + playerMomentum);

            //change state
            playerController.playerState = PlayerController.PlayerState.jumping;

            //jump
            if (!holdingPlank)
            {
                jumpVector = new Vector3(0, jumpHeight, 0);
            }
            else
            {
                jumpVector = new Vector3(0, holdingBoardJumpHeight, 0);
            }
            return jumpVector;
        }
        else
        {
            //jumpVector = new Vector3(0, jumpHeight, 0);
            return Vector3.zero;
        }
    }

    public Vector3 Gravity()
    {
        //while player is off ground increase gravity
        /*if (playerController.IsPlayerGrounded())
        {
            //is grounded
            gravityVector = Vector3.zero;
        }
        else if (gravityVector == Vector3.zero)
        {
            gravityVector = new Vector3(0, gravity, 0);
        }
        else
        {
            gravityVector *= 2;
        }*/
        if (playerController.PlayerGrounded)
        {
            return Vector3.zero;
        }
        else
        {
            return gravityVector;
        }
    }

    public Vector3 ApplyDrag(Vector3 directionMoving, bool grounded)
    {
        if (grounded)
        {
            //ground drag
            //take x and z
            //reduce them by drag percentage
            directionMoving.x *= (1 - groundDrag);
            directionMoving.z *= (1 - groundDrag);

        }
        else
        {
            //airdrag
            //take x and z
            //reduce them by drag percentage
            directionMoving.x *= (1 - airDrag);
            directionMoving.z *= (1 - airDrag);
        }
        return directionMoving;
    }

    public void ResetMovement()
    {
        playerMomentum = Vector3.zero;
        reversePlayerMovementFromJoysticks = Vector3.zero;
    }

}
