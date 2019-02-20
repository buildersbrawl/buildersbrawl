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
    public float postJumpCooldown = 0f; //OBSELETE?
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

    private Vector3 playerJumpMomentum;
    private Vector3 playerCombatMomentum;
    private Vector3 playerEnvironmentMomentum;
    


    [SerializeField]
    private float groundDrag = .1f;
    [SerializeField]
    private float airDrag = 0f;

    //can use to get player momentum
    private Vector3 reversePlayerMovementFromJoysticks;

    private Vector3 addedMomentumFromEnvironment;
    public Vector3 EnviromentMomentum
    {
        set
        {
            addedMomentumFromEnvironment = value;
        }
    }

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

            //and add small amount of input direction
            playerFinalDirection = (direction * playerSpeed) * slowDownSpeedPercentage;
            reversePlayerMovementFromJoysticks = ((direction * playerSpeed) * slowDownSpeedPercentage) * -1;

            //TODO: figure out how to stop player from doing accelerated jump
            //clamp magnitude so players cannot accelerate past momentum
            //Mathf.Clamp(playerFinalDirection.magnitude, 0, playerMomentum.magnitude);
        }
        else
        {

            reversePlayerMovementFromJoysticks = (direction * playerSpeed) * -1;
            playerFinalDirection = direction * playerSpeed;

        }
            
        return playerFinalDirection;
    }

    public Vector3 AddPlayerMomentum(Vector3 direction)
    {
        playerMomentum = direction;

        return playerMomentum;
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
    /*
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
    */
    public void ApplyDrag(bool grounded)
    {
        if (grounded)
        {
            //ground drag
            //reduce them by drag percentage
            playerCombatMomentum *= (1 - groundDrag);
            playerEnvironmentMomentum *= (1 - groundDrag);
            playerJumpMomentum *= (1 - groundDrag);
        }
        else
        {
            //airdrag
            //reduce them by drag percentage
            playerCombatMomentum *= (1 - airDrag);
            playerEnvironmentMomentum *= (1 - airDrag);
            playerJumpMomentum *= (1 - airDrag);
        }
    }


    public void AddEnvironmentMomentum()
    {
        playerMomentum += addedMomentumFromEnvironment;
    }

    public void ResetMovement()
    {
        playerMomentum = Vector3.zero;
        reversePlayerMovementFromJoysticks = Vector3.zero;
    }

    public void PushMe(Vector3 pushDirection, float pushForce)
    {
        print("I got pushed " + this.gameObject.name);
        playerCombatMomentum = pushDirection * pushForce;

        if (playerController.playerActions.HeldPlank != null)
        {
            playerController.playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.drop);
        }
    }

    public void JumpEnd()
    {
        playerJumpMomentum = Vector3.zero;
    }

    public void CalculateMomentum()
    {
        //collect all momentums += 
        //add them to momentum +=
        //apply full momentum to moveVector
        //drag to parts momentum 
        //reset full momentum = Vecotr3.zero

        playerMomentum = Vector3.zero;

        //jump momentum
        playerMomentum += playerJumpMomentum;

        //combat momentum
        playerMomentum += playerCombatMomentum;

        //environment
        playerMomentum += playerEnvironmentMomentum;
    }

}
