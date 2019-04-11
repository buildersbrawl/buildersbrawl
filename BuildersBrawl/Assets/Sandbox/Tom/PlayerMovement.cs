using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //player controller ref
    PlayerController playerController;

    private float playerSpeed = 3.5f;
    [Tooltip("What percentage the player slows down when jumping")]
    [Range(0,1)]
    private float slowDownJumpSpeedPercentage = .9f;
    [Tooltip("What percentage the player slows down when holdingplank")]
    [Range(0, 1)]
    private float slowDownHoldingPlankSpeedPercentage = .2f;

    Vector3 playerFinalDirection;

    [Header("Jumping Variables")]
    [Tooltip("Starts when player returns to ground (OBSELETE)")]
    private float postJumpCooldown = 0f; //OBSELETE?
    private float jumpHeight = 7f;
    private float holdingBoardJumpHeight = 3f;
    private Vector3 jumpVector;

    [Header("Physics")]
    public float gravity = -.5f;
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
    //[SerializeField]
    private Vector3 playerEnvironmentMomentum;
    


    //[SerializeField]
    private float groundDrag = .1f;
    //[SerializeField]
    private float airDrag = 0f;

    //can use to get player momentum
    private Vector3 reversePlayerMovementFromJoysticks;


    [Header("WackMovement")]
    public bool addWackyMovement = false;
    private float rangeTillChangeInDegrees = 15f;
    [Tooltip("How extreme the lean is. 1 = lean constant")]
    private float leanFactor = 1.05f;
    //[SerializeField]
    //public float leanStartTest; //obselete
    private float leanAmount; //starts as somewhere between Vector3(1, 0, 0) and Vector3(-1, 0, 0)
    //[SerializeField]
    private float leanCeiling = 180f; //to stop player from spinning so fast it just looks weird
    //[SerializeField]
    private float leanAddOnceHitCeiling = 5f; //to stop player from spinning so fast it just looks weird
    private Vector3 joyInputOriginalDirection;
    private Vector3 joyInputRangeLeft, joyInputRangeRight;
    //if player only minimally using joystick give them full control
    private float lowInputNumberSoNoWackyMovement = .5f;

    [Header("WobbleMovement")]
    public bool addWobble = true;
    [Range(0, 1)]
    [Tooltip("Must be between 0 and 1")]
    public float wobbleSpeed = .01f;
    private float wobbleAmount;
    private float sway = .5f;
    private float maxWobbleAngleInDegrees = 30f;
    private bool swaySwitch;
    private Vector3 lastKnownJoyDirection;
    public bool noWobbleOnMove = true;

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
        if(playerController.playerState == PlayerController.PlayerState.jumping)
        {

            //and add small amount of input direction
            playerFinalDirection = (direction * playerSpeed) - ((direction * playerSpeed) * slowDownJumpSpeedPercentage);
            reversePlayerMovementFromJoysticks = ((direction * playerSpeed)- ((direction * playerSpeed) * slowDownJumpSpeedPercentage)) * -1;

            //TODO: figure out how to stop player from doing accelerated jump
            //clamp magnitude so players cannot accelerate past momentum
            //Mathf.Clamp(playerFinalDirection.magnitude, 0, playerMomentum.magnitude);
        }
        else if(playerController.playerActions.HeldPlank != null)
        {
            //and add small amount of input direction
            playerFinalDirection = (direction * playerSpeed) - ((direction * playerSpeed) * slowDownHoldingPlankSpeedPercentage);
            reversePlayerMovementFromJoysticks = ((direction * playerSpeed) - ((direction * playerSpeed) * slowDownHoldingPlankSpeedPercentage)) * -1;
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
                                                                //temp solution
            playerJumpMomentum = playerController.LastFramesMoveVector;
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
        //TODO: add if mometum lower than certain number just make 0

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

    public void ResetMovement()
    {
        //playerCombatMomentum = Vector3.zero;
        playerJumpMomentum = Vector3.zero;
        playerEnvironmentMomentum = Vector3.zero;
        playerMomentum = Vector3.zero;
        reversePlayerMovementFromJoysticks = Vector3.zero;
        if (playerController.playerDeath.playerDead)
        {
            playerCombatMomentum = Vector3.zero;
        }
    }

    public void PushMe(Vector3 pushDirection, float pushForce)
    { 
        //wait to see if this player dies, if he does then give points to the other other player
        StartCoroutine(this.GetComponent<PlayerDeath>().WaitForDeathToHappen());

        //call animation
        playerController.playerAnimation.PushedAnim(pushDirection);

        print("I got pushed " + this.gameObject.name);
        playerCombatMomentum = pushDirection * pushForce;

        if (playerController.playerActions.HeldPlank != null)
        {
            playerController.playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.drop);
        }

        playerController.playerState = PlayerController.PlayerState.pushed;

        StartCoroutine(EndPushed());
    }

    //stops player cooldown when their combat momentum gets very low
    IEnumerator EndPushed()
    {
        yield return new WaitForSeconds(.01f);
        if(playerCombatMomentum.magnitude < .01f)
        {
            playerController.playerState = PlayerController.PlayerState.defaultMovement;
        }
        else
        {
            //print("Player pushed");
            StartCoroutine(EndPushed());
        }
    }

    public void JumpEnd()
    {
        playerJumpMomentum = Vector3.zero;
    }

    public void SetEnvironmentMomentum(Vector3 newMomentum)
    {
        playerEnvironmentMomentum = newMomentum;
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

    //--------------------------------------------------------------------------------------
    //WACK
    //

    //for some reason every other frame joyinput is 0

    public void StartWackMovement(Vector3 joystickInput)
    {
        //take vector joystick is at (this will be the center)
        joyInputOriginalDirection = joystickInput;

        print("new lean amount");
        //set lean
        leanAmount = Random.Range(-1f, 1f);
        //testing
       // leanAmount = leanStartTest;
        //print("leanVector " + leanVector);
    }

    public Vector3 UpdateWackMovement(Vector3 joystickInput)
    {
        //if input too low or out of range
        if((Mathf.Abs(joystickInput.x) < lowInputNumberSoNoWackyMovement) && (Mathf.Abs(joystickInput.z) < lowInputNumberSoNoWackyMovement))
        {
            //print("joy too low" + joystickInput);
            //no wack movement
            //set new wack move stuff
            StartWackMovement(joystickInput);
        }
        else if(!(CheckPointInPieSlice(joyInputOriginalDirection, rangeTillChangeInDegrees, joystickInput)))
        {
            //print("joy not in right spot");
            //set new wack move stuff
            StartWackMovement(joystickInput);
        }
        //else if still in range
        else
        {
            //print("lean" + joystickInput);
            //else take current lean vector and multiply it by leanFactor
            //else{

            if(Mathf.Abs(leanAmount * leanFactor) > leanCeiling)
            {
                if(leanAmount > 0)
                {
                    leanAmount += leanAddOnceHitCeiling;
                }
                else
                {
                    leanAmount -= leanAddOnceHitCeiling;
                }
                
            }
            else
            {
                leanAmount *= leanFactor;
            }
            
            /*if(leanAmount > 360)
            {
                leanAmount = leanAmount - 360;
            }
            */
            
            

            //add lean vector to input
            joystickInput = Quaternion.AngleAxis(leanAmount, Vector3.up) * joystickInput;
        }

        return joystickInput;
    }

    private bool CheckPointInPieSlice(Vector3 center, float range, Vector3 spotInQuestion)
    {
        bool inRange = true;

        //make sure angle between center and spot in question is less than range
        
        if(!(Mathf.Abs(Vector3.Angle(center, spotInQuestion)) < Mathf.Abs(range)))
        {
            //if not not inRange
            inRange = false;
        }
        //make sure distance between the point and the center is less than or equal to the radius (which is 1)
        if(!(Mathf.Abs(Vector3.Distance(center, spotInQuestion)) <= 1f))
        {
            //if not not inRange
            inRange = false;
        }

        return inRange;
    }

    public Vector3 UpdateWobbleMovement(Vector3 joystickInput)
    {
        if (swaySwitch)
        {
            sway += wobbleSpeed;
            if(sway > 1)
            {
                swaySwitch = false;
            }
        }
        else
        {
            sway -= wobbleSpeed;
            if (sway < 0)
            {
                swaySwitch = true;
            }
        }
        //make wobble go between degree
        //2
        //1.5
        //

        wobbleAmount = Mathf.Lerp(maxWobbleAngleInDegrees, -maxWobbleAngleInDegrees, sway);

        if(joystickInput != Vector3.zero)
        {
            lastKnownJoyDirection = joystickInput;
            lastKnownJoyDirection.Normalize();
            lastKnownJoyDirection *= .001f;

            if (noWobbleOnMove)
            {
                wobbleAmount = 0;
            }
        }
        else
        {
            joystickInput = lastKnownJoyDirection;
        }

        //add lean vector to input
        joystickInput = Quaternion.AngleAxis(wobbleAmount, Vector3.up) * joystickInput;

        return joystickInput;
    }


}
