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
    [Tooltip("Starts when player returns to ground")]
    public float postJumpCooldown = .2f;
    //public bool canJump = true;

    public float jumpHeight = 1f;
    private Vector3 jumpVector;

    public float gravity = -8f;
    private Vector3 gravityVector;

    private Vector3 playerMomentum;

    public Vector3 reversePlayerMovementFromJoysticks;

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

            //TODO: figure out how to stop player from doing accelerated jump
            //clamp magnitude so players cannot accelerate past momentum
            //Mathf.Clamp(playerFinalDirection.magnitude, 0, playerMomentum.magnitude);
        }
        else
        {
            playerFinalDirection = direction * playerSpeed;

        }
        reversePlayerMovementFromJoysticks = (playerFinalDirection * -1);
        return playerFinalDirection;
    }

    public Vector3 Jump(bool buttonPressed)
    {
        if (buttonPressed)
        {
            //print("Jump button pressed");

            //record player momentum
            playerMomentum = playerController.LastFramesMoveVector;

            //change state
            playerController.playerState = PlayerController.PlayerState.jumping;

            //jump
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

}
