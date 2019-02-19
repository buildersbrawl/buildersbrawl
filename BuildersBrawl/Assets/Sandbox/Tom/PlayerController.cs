using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum InputType
    {
        controller,
        keyboard
    }

    public enum PlayerNumber
    {
        p1,
        p2
    }
    
    //TODO: Add rotation to char controller so facing direction moving

    public enum PlayerState
    {
        defaultMovement,
        jumping,
        action,
        holdingPlank,
        cooldown
    }

    [Header("Input")]
    public GameInputManager gameInputManager;

    public InputType inputType;

    public PlayerNumber playerNumber;

    public CameraController cameraRef;

    [Header("Player info")]
    public PlayerState playerState;

    //Char cont reference
    CharacterController charContRef;

    [HideInInspector]
    //player actions ref
    public PlayerActions playerActions;

    [HideInInspector]
    //player move ref
    public PlayerMovement playerMovement;

    [HideInInspector]
    //player death ref
    public PlayerDeath playerDeath;

    //determines player movement
    private Vector3 moveVector;

    private Vector3 lastFramesMoveVector;
    public Vector3 LastFramesMoveVector
    {
        get
        {
            return lastFramesMoveVector;
        }
    }


    //[Header("For Testing")]
    private bool right;
    private bool left;
    private bool forward;
    private bool backwards;
    private bool AJump;
    private bool BCharge;
    private bool XPush;
    private bool YPickOrPlace;
    private bool dropPlankControl;
    private bool BumpOrTrigSlam;

    private Vector3 joyInput;

    [Header("Bounce")]
    public float bounciness = 1f;
    [SerializeField]
    private float collisonCheckLength;
    [Tooltip("The more drag the faster the players excess momentum is stopped. Value between 0-1.")]
    

    [Header("Grounding")]

    [SerializeField]
    private float groundCheckDistance = 2f;
    [SerializeField]
    private bool playerGrounded;

    public bool PlayerGrounded
    {
        get
        {
            return playerGrounded;
        }
    }

    //around cos(pi/4) or sin (pi/4) (they're the same number)
    private const float TURN_INPUT_45_DEGREES_CONSTANT = 0.70710678118f;

    private Vector3 directionPlayerFacing; //same as x and z of direction moving

    [HideInInspector]
    public bool addWackyMovement;


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        //get char cont
        if (this.gameObject.GetComponent<CharacterController>() != null)
        {
            charContRef = this.gameObject.GetComponent<CharacterController>();
        }
        else
        {
            charContRef = this.gameObject.AddComponent<CharacterController>();
        }

        //get char cont
        if (this.gameObject.GetComponent<GameInputManager>() != null)
        {
            gameInputManager = this.gameObject.GetComponent<GameInputManager>();
        }
        else
        {
            gameInputManager = this.gameObject.AddComponent<GameInputManager>();
        }


        //get move cont
        if (this.gameObject.GetComponent<PlayerMovement>() != null)
        {
            playerMovement = this.gameObject.GetComponent<PlayerMovement>();
        }
        else
        {
            playerMovement = this.gameObject.AddComponent<PlayerMovement>();
        }
        playerMovement.InitMove(this);

        //get action cont
        if (this.gameObject.GetComponent<PlayerActions>() != null)
        {
            playerActions = this.gameObject.GetComponent<PlayerActions>();
        }
        else
        {
            playerActions = this.gameObject.AddComponent<PlayerActions>();
        }
        playerActions.InitAct(this);

        //get action cont
        if (this.gameObject.GetComponent<PlayerDeath>() != null)
        {
            playerDeath = this.gameObject.GetComponent<PlayerDeath>();
        }
        else
        {
            playerDeath = this.gameObject.AddComponent<PlayerDeath>();
        }

        //camera
        if (cameraRef == null)
        {
            cameraRef = GameObject.FindObjectOfType<CameraController>();
        }

        playerState = PlayerState.defaultMovement;

        //test
        joyInput = Vector3.zero;

    }

    //keyboard input
    private void KeyboardInput()
    {
        //temp

        if(playerNumber == PlayerNumber.p1)
        {
            if (Input.GetKey(KeyCode.D))
            {
                right = true;
            }
            else
            {
                right = false;
            }
            if (Input.GetKey(KeyCode.A))
            {
                left = true;
            }
            else
            {
                left = false;
            }
            if (Input.GetKey(KeyCode.W))
            {
                forward = true;
            }
            else
            {
                forward = false;
            }
            if (Input.GetKey(KeyCode.S))
            {
                backwards = true;
            }
            else
            {
                backwards = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AJump = true;
            }
            else
            {
                AJump = false;
            }
            if (Input.GetKey(KeyCode.E))
            {
                //print("hit e");
                YPickOrPlace = true;
            }
            else
            {
                YPickOrPlace = false;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                //print("hit q");
                dropPlankControl = true;
            }
            else
            {
                dropPlankControl = false;
            }
            if (Input.GetKey(KeyCode.F))
            {
                //print("hit q");
                XPush = true;
            }
            else
            {
                XPush = false;
            }
            if (Input.GetKey(KeyCode.B))
            {
                //print("hit q");
                BCharge = true;
            }
            else
            {
                BCharge = false;
            }

        }
        else if (playerNumber == PlayerNumber.p2)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                right = true;
            }
            else
            {
                right = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                left = true;
            }
            else
            {
                left = false;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                forward = true;
            }
            else
            {
                forward = false;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                backwards = true;
            }
            else
            {
                backwards = false;
            }
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                AJump = true;
            }
            else
            {
                AJump = false;
            }
            if (Input.GetKey(KeyCode.Keypad1))
            {
                YPickOrPlace = true;
            }
            else
            {
                YPickOrPlace = false;
            }
            if (Input.GetKey(KeyCode.Keypad2))
            {
                BCharge = true;
            }
            else
            {
                BCharge = false;
            }
            if (Input.GetKey(KeyCode.Keypad3))
            {
                //print("hit q");
                XPush = true;
            }
            else
            {
                XPush = false;
            }


        }
        else
        {
            print("Error: no control scheme for this player number");
        }


        if (right)
        {
            joyInput += (new Vector3(1, 0, 0));
        }
        if (left)
        {
            joyInput += (new Vector3(-1, 0, 0));
        }
        if (forward)
        {
            joyInput += (new Vector3(0, 0, 1));
        }
        if (backwards)
        {
            joyInput += (new Vector3(0, 0, -1));
        }


    }

    //controller input
    private void ControllerInput()
    {
        //input
        joyInput = gameInputManager.joystickInput;
        AJump = gameInputManager.pressedJumpButton;
        BCharge = gameInputManager.pressedChargeButton;
        XPush = gameInputManager.pressedPushButton;
        YPickOrPlace = gameInputManager.pressedBoardPickUpOrDropButton;
        BumpOrTrigSlam = gameInputManager.pressedSlamButton;

    }

    //steady frames for physics and movement
    public void FixedUpdate()
    {

        //Input fix?
        ///-------------------------------------------------------------------------------------
        if (inputType == InputType.keyboard)
        {
            KeyboardInput();
        }
        else if (inputType == InputType.controller)
        {
            ControllerInput();
        }

        if (cameraRef != null)
        {
            if (cameraRef.cameraOptions == CameraController.CameraOptions.side)
            {
                joyInput = ConvertJoystickInputToSide(joyInput);
            }
            else if (cameraRef.cameraOptions == CameraController.CameraOptions.front)
            {
                joyInput = ConvertJoystickInputToFront(joyInput);
            }
            else if (cameraRef.cameraOptions == CameraController.CameraOptions.fortyFiveDegrees)
            {
                joyInput = ConvertJoystickInput45Degrees(joyInput);
            }
        }

        //------------------------------------------------------------------------------------------


        //if player dead stop control
        if (playerDeath.playerDead)
        {
            moveVector = Vector3.zero;
            playerMovement.ResetMovement();
            return;
        }

        //temp
        //if someone won then if "a" hit restart
        if (GameManager.S != null && GameManager.S.someoneWon && BCharge)
        {
            GameManager.S.RestartGame();
        }


            //check to see if just hit the ground from jumping
        if (playerGrounded && playerState == PlayerState.jumping)
        {
            //start jump cooldown
            StartCoroutine(ReturnPlayerStateToMoving(playerMovement.postJumpCooldown));
        }

        //if not doing something else and grounded a will activate jump
        if (playerState != PlayerState.action && playerState != PlayerState.jumping && playerGrounded)
        {
            //a jump                                        //true if holding plank
            moveVector += playerMovement.Jump(AJump, playerActions.HeldPlank != null);
        }

        //print("After jump state is " + playerState);

        //IF JUMPING OVERRIDE ACTION or if doing other action
        if (playerState != PlayerState.jumping && playerState != PlayerState.cooldown)
        {
            //y pick up
            //y drop board
            if (YPickOrPlace)
            {
                //print("action");
                if (playerActions.HeldPlank != null) //holding board
                {
                    //place
                    playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.place);
                }
                else //NOT holding board
                {
                    //pick up
                    playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.pickUp);
                }

            }
            //x is push
            else if (XPush)
            {
                playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.push);
            }
            //b is charge
            else if (BCharge)
            {
                playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.charge);
            }
            //bumpers are board slam
            else if (BumpOrTrigSlam)
            {
                playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.slam);
            }
            else if (dropPlankControl)
            {
                //drop
                playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.drop);
            }
        }

        //move based off of joystick, if performing action don't do this
        if (playerState != PlayerState.action)
        {
            //call player movement based off of joystick movement
            moveVector += playerMovement.PlayerSideMovement(joyInput, playerState);
        }
        else
        {
            //reset movement
            playerMovement.ResetMovement();
        }

        //apply gravity
        moveVector += playerMovement.Gravity() * Time.fixedDeltaTime;

        //print("After update state is " + playerState);

        //print(moveVector);


        //-------------------------
        //apply movement
        charContRef.Move(moveVector * Time.fixedDeltaTime);



        //apply rotation
        //make player look where input rotation is

        //Vector3 moveVectorLimitY = moveVector;
        //moveVectorLimitY.y = 0;

        //if (moveVectorLimitY != Vector3.zero)
        if(joyInput != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(joyInput, Vector3.up), 0.15f);
        }
        //----------------------------


        //see if grounded
        playerGrounded = IsPlayerGrounded();

        //next frame figure out collision
        //moveVector += CollisionCheck(moveVector);

        //print("Groundid is: " + playerGrounded);

        //before reset record last vector
        lastFramesMoveVector = moveVector;

        //reset moveVector by getting rid of impact of joystick input
        //moveVector += playerMovement.reversePlayerMovementFromJoysticks;
        //now if there extra x/z movement (momentum) reduce it with drag
        //moveVector = ApplyDrag(moveVector);

        //add drag to momentum
        playerMovement.PlayerMomentum = playerMovement.ApplyDrag(playerMovement.PlayerMomentum, playerGrounded);


        //old way
        moveVector.x = 0;
        moveVector.z = 0;

        //stop pushing player down if on ground
        if (playerGrounded)
        {
            moveVector.y = 0;
        }

       

        //Mathf.Clamp(moveVector.x, 0, 0);
        //Mathf.Clamp(moveVector.z, 0, 0);
        /*if (playerGrounded)
        {
            moveVector.y = 0;
        }
        else
        {
            Mathf.Clamp(moveVector.y, -10, 10);
        }*/


        //temp reset input vector
        joyInput = Vector3.zero;
    }


    //TODO: add more raycasts for accuracy

    public bool IsPlayerGrounded()
    {
        RaycastHit groundInfo;

        //raycast down short distance, if hits anything grounded
        //Debug.DrawRay(this.gameObject.transform.position, Vector3.down, Color.red, groundCheckDistance);
        if(Physics.Raycast(this.gameObject.transform.position, Vector3.down, out groundInfo, groundCheckDistance)) //out groundInfo
        {
            //if didn't hit trigger
            if (!groundInfo.collider.isTrigger)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        //return charContRef.isGrounded;
    }

    public IEnumerator ReturnPlayerStateToMoving(float waitTime)
    {
        playerState = PlayerState.cooldown;
        yield return new WaitForSeconds(waitTime);
        //reset state
        playerState = PlayerState.defaultMovement;
    }

    //takes input and rotates it 45 degrees to match the 
    public Vector3 ConvertJoystickInput45Degrees(Vector3 input)
    {
        Vector3 answer = Vector3.zero;

        //used these equations to rotate things on a 2d plane. Thank you stack overflow: https://stackoverflow.com/questions/14607640/rotating-a-vector-in-3d-space
        //newX = x cos θ − y sin θ
        //newY = x sin θ + y cos θ

        answer.x = (input.x * TURN_INPUT_45_DEGREES_CONSTANT) - (input.z * TURN_INPUT_45_DEGREES_CONSTANT);
        answer.z = (input.x * TURN_INPUT_45_DEGREES_CONSTANT) + (input.z * TURN_INPUT_45_DEGREES_CONSTANT);

        return answer;
    }

    public Vector3 ConvertJoystickInputToSide(Vector3 input)
    {
        Vector3 answer = Vector3.zero;

        answer.x = -joyInput.z;
        answer.z = joyInput.x;

        return answer;
    }

    public Vector3 ConvertJoystickInputToFront(Vector3 input)
    {
        Vector3 answer = Vector3.zero;

        answer.x = -joyInput.x;
        answer.z = -joyInput.z;

        return answer;
    }


    private Vector3 CollisionCheck(Vector3 directionMoving)
    {
        //raycast in direction moving
        RaycastHit hitInfo;
        Vector3 reflection = Vector3.zero;

        //TODO: add more raycasts to this
        if (Physics.Raycast(this.gameObject.transform.position, directionMoving, out hitInfo, collisonCheckLength))
        {
            print("hit");
            //if hit take reflection of direction moving and surface hit
            reflection = Vector3.Reflect(directionMoving, hitInfo.normal) * bounciness;
        }

        //return reflection
        return reflection;
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 reflection = Vector3.zero;
        print("hit collider");
        //if hit take reflection of direction moving and surface hit
        reflection = Vector3.Reflect(moveVector, collision.GetContact(0).normal) * bounciness;

        moveVector += reflection;
    }
    */


    

    public void PushMe(Vector3 pushDirection, float pushForce)
    {
        print("I got pushed " + this.gameObject.name);
        playerMovement.PlayerMomentum += pushDirection * pushForce;

        if(playerActions.HeldPlank != null)
        {
            playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.drop);
        }
    }

}

