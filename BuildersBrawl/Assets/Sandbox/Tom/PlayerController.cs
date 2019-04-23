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
        p1Clumsy,
        p2Tough,
        p3Joker,
        p4Crazy
    }
    
    //TODO: Add rotation to char controller so facing direction moving

    public enum PlayerState
    {
        defaultMovement,
        jumping,
        action,
        holdingPlank,
        cooldown,
        stunned,
        pushed
    }

    private bool stunSoDontStopFromCooldown;

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

    [HideInInspector]
    //score
    public Points playerPoints;

    [HideInInspector]
    //score
    public PlayerAnimation playerAnimation;

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
    private bool XPickOrPlace;
    private bool YPickOrPlace;
    private bool dropPlankControl;
    private bool BumpOrTrigSlamOrPush;
    private bool StartPause;

    private Vector3 joyInput;

    [Header("Bounce (Deprecated)")]
    //[SerializeField]
    private float bounciness = 1f;
    //[SerializeField]
    private float collisonCheckLength;
    

    [Header("Grounding")]
    //[SerializeField]
    private float groundCheckDistance = 1.1f;
    //[SerializeField]
    private float groundCheckSize = .3f;
    //[SerializeField]
    private bool playerGrounded;

    public bool PlayerGrounded
    {
        get
        {
            return playerGrounded;
        }
    }

    [Header("Jump")]
    [HideInInspector]
    public bool jumpEnabled = false;

    //around cos(pi/4) or sin (pi/4) (they're the same number)
    private const float TURN_INPUT_45_DEGREES_CONSTANT = 0.70710678118f;

    private Vector3 directionPlayerFacing; //same as x and z of direction moving

    //[Header("Other")]
    //[SerializeField]
    //private float flattenPercent = .2f;
    //[SerializeField]
    //private float flattenDownAmount = .4f;

   // [HideInInspector]
    public bool tempStopMovement;

    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    private void Awake()
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

        //get anim
        if (this.gameObject.GetComponent<PlayerAnimation>() != null)
        {
            playerAnimation = this.gameObject.GetComponent<PlayerAnimation>();
        }
        else
        {
            playerAnimation = this.gameObject.AddComponent<PlayerAnimation>();
        }
        playerAnimation.Init();

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

        if (playerNumber == PlayerNumber.p1Clumsy)
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                //print("hit e");
                YPickOrPlace = true;
            }
            else
            {
                YPickOrPlace = false;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //print("hit q");
                dropPlankControl = true;
            }
            else
            {
                dropPlankControl = false;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                //print("hit q");
                BumpOrTrigSlamOrPush = true;
            }
            else
            {
                BumpOrTrigSlamOrPush = false;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                //print("hit q");
                BCharge = true;
            }
            else
            {
                BCharge = false;
            }
        }
        else if (playerNumber == PlayerNumber.p2Tough)
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
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKey(KeyCode.P))
            {
                YPickOrPlace = true;
            }
            else
            {
                YPickOrPlace = false;
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                BCharge = true;
            }
            else
            {
                BCharge = false;
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                //print("hit q");
                BumpOrTrigSlamOrPush = true;
            }
            else
            {
                BumpOrTrigSlamOrPush = false;
            }
        }

        else if (playerNumber == PlayerNumber.p3Joker)
        {
            if (Input.GetKey(KeyCode.L))
            {
                right = true;
            }
            else
            {
                right = false;
            }
            if (Input.GetKey(KeyCode.J))
            {
                left = true;
            }
            else
            {
                left = false;
            }
            if (Input.GetKey(KeyCode.I))
            {
                forward = true;
            }
            else
            {
                forward = false;
            }
            if (Input.GetKey(KeyCode.K))
            {
                backwards = true;
            }
            else
            {
                backwards = false;
            }

        }
        else if (playerNumber == PlayerNumber.p4Crazy)
        {
            if (Input.GetKey(KeyCode.H))
            {
                right = true;
            }
            else
            {
                right = false;
            }
            if (Input.GetKey(KeyCode.F))
            {
                left = true;
            }
            else
            {
                left = false;
            }
            if (Input.GetKey(KeyCode.T))
            {
                forward = true;
            }
            else
            {
                forward = false;
            }
            if (Input.GetKey(KeyCode.G))
            {
                backwards = true;
            }
            else
            {
                backwards = false;
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
        BumpOrTrigSlamOrPush = gameInputManager.pressedPushButton;
        YPickOrPlace = gameInputManager.pressedBoardPickUpOrDropButton;
        BumpOrTrigSlamOrPush = gameInputManager.pressedSlamButton;
        StartPause = gameInputManager.pressedStart;

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

        //if they press start, bring up pause menu
        if (StartPause)
        {
            //Debug.Log("Paused - PlayerController");
            Pause.S.PauseGame();
        }


        //if player dead or stunned stop control
        if (playerDeath.playerDead || playerState == PlayerState.stunned)
        {
            moveVector = Vector3.zero;
            playerMovement.ResetMovement();
            joyInput = Vector3.zero;
            return;
        }

        //temp
        //if someone won then if "b" hit restart
        if (GameManager.S != null && GameManager.S.someoneWon && BCharge)
        {
            GameManager.S.EndLevel();
        }


            //check to see if just hit the ground from jumping
        if (playerGrounded && playerState == PlayerState.jumping)
        {
            playerMovement.JumpEnd();

            //start jump cooldown
            //StartCoroutine(ReturnPlayerStateToMoving(playerMovement.postJumpCooldown));
            playerState = PlayerState.defaultMovement; //this should fix the infinite upwards jump
        }

        //if not doing something else and grounded a will activate jump
        if (playerState != PlayerState.action && playerState != PlayerState.jumping && playerGrounded)
        {
            //a jump

            if (jumpEnabled)
            {
                //true if holding plank
                moveVector += playerMovement.Jump(AJump, playerActions.HeldPlank != null);
            }
        }

        //print("After jump state is " + playerState);

        //print("frame " + joyInput);

        //IF JUMPING OVERRIDE ACTION or if doing other action or if anim stop
        if (playerState != PlayerState.jumping && playerState != PlayerState.cooldown && playerState != PlayerState.pushed && !tempStopMovement)
        {
            //y pick up
            //y drop board
            if (YPickOrPlace)
            {
                //print("PickOrPlace pressed");
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
            //bumpers are board slam or push
            else if (BumpOrTrigSlamOrPush && playerActions.HeldPlank == null)
            {
                //print("pressed push");
                playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.push);
            }
            //bumpers are board slam or push
            else if (BumpOrTrigSlamOrPush && playerActions.HeldPlank != null)
            {
                //print("pressed slam");
                playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.slam);
            }
            //b is charge
            else if (BCharge)
            {
                playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.charge);
            }
            else if (dropPlankControl)
            {
                //drop
                playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.drop);
            }
        }

        //move based off of joystick
        if (!tempStopMovement)
        {
            if (playerMovement.addWackyMovement)
            {
                joyInput = playerMovement.UpdateWackMovement(joyInput);
            }
            if (playerMovement.addWobble)
            {
                joyInput = playerMovement.UpdateWobbleMovement(joyInput);
            }

            //call player movement based off of joystick movement
            moveVector += playerMovement.PlayerSideMovement(joyInput, playerState);
            //moveVector += playerMovement.AddPlayerMomentum(joyInput);

            playerAnimation.RunAnim(moveVector);
        }
        else
        {
            //stop player momentum and joystick movement if performing action
            playerMovement.ResetMovement();
        }

        //apply gravity
        moveVector += playerMovement.Gravity();


        //MOMENTUM
        //-------------------------
        //do all the changes to momentum

        playerMovement.CalculateMomentum();

        //apply momentum
        moveVector += playerMovement.PlayerMomentum;
        //-------------------------


        //print("After update state is " + playerState);

        //print(moveVector);




        //-------------------------
        //apply movement
        charContRef.Move(moveVector * Time.fixedDeltaTime);
        //----------------------------

        //animation
        


        //apply rotation
        //make player look where input rotation is

        //Vector3 moveVectorLimitY = moveVector;
        //moveVectorLimitY.y = 0;

        //if (moveVectorLimitY != Vector3.zero)
        //player rotation
        if (joyInput != Vector3.zero && !tempStopMovement)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(joyInput, Vector3.up), 0.15f);
        }
        


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

        //playerMovement.PlayerMomentum = playerMovement.ApplyDrag(playerMovement.PlayerMomentum, playerGrounded);

        playerMovement.ApplyDrag(playerGrounded);


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
        ///Debug.DrawRay(this.gameObject.transform.position, Vector3.down, Color.red, groundCheckDistance);
        //Debug.DrawRay(this.gameObject.transform.position + new Vector3(this.gameObject.transform.localScale.x * groundCheckSize, 0, this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, Color.red, groundCheckDistance);
        //Debug.DrawRay(this.gameObject.transform.position + new Vector3(-this.gameObject.transform.localScale.x * groundCheckSize, 0, this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, Color.red, groundCheckDistance);
        //Debug.DrawRay(this.gameObject.transform.position + new Vector3(this.gameObject.transform.localScale.x * groundCheckSize, 0, -this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, Color.red, groundCheckDistance);
        //Debug.DrawRay(this.gameObject.transform.position + new Vector3(-this.gameObject.transform.localScale.x * groundCheckSize, 0, -this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, Color.red, groundCheckDistance);
        ///Debug.DrawRay(this.gameObject.transform.position + new Vector3(this.gameObject.transform.localScale.x * groundCheckSize, 0, 0), Vector3.down, Color.red, groundCheckDistance);
        ///Debug.DrawRay(this.gameObject.transform.position + new Vector3(-this.gameObject.transform.localScale.x * groundCheckSize, 0, 0), Vector3.down, Color.red, groundCheckDistance);
        //Debug.DrawRay(this.gameObject.transform.position + new Vector3(0, 0, this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, Color.red, groundCheckDistance);
        //Debug.DrawRay(this.gameObject.transform.position + new Vector3(0, 0, -this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, Color.red, groundCheckDistance);

        if (Physics.Raycast(this.gameObject.transform.position, Vector3.down, out groundInfo, groundCheckDistance) 
            //|| Physics.Raycast(this.gameObject.transform.position + new Vector3(this.gameObject.transform.localScale.x * groundCheckSize, 0, this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, out groundInfo, groundCheckDistance) 
            //|| Physics.Raycast(this.gameObject.transform.position + new Vector3(-this.gameObject.transform.localScale.x * groundCheckSize, 0, this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, out groundInfo, groundCheckDistance) 
            //|| Physics.Raycast(this.gameObject.transform.position + new Vector3(this.gameObject.transform.localScale.x * groundCheckSize, 0, -this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, out groundInfo, groundCheckDistance)
            //|| Physics.Raycast(this.gameObject.transform.position + new Vector3(-this.gameObject.transform.localScale.x * groundCheckSize, 0, -this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, out groundInfo, groundCheckDistance)
            || Physics.Raycast(this.gameObject.transform.position + new Vector3(this.gameObject.transform.localScale.x * groundCheckSize, 0, 0), Vector3.down, out groundInfo, groundCheckDistance)
            || Physics.Raycast(this.gameObject.transform.position + new Vector3(-this.gameObject.transform.localScale.x * groundCheckSize, 0, 0), Vector3.down, out groundInfo, groundCheckDistance)
            //|| Physics.Raycast(this.gameObject.transform.position + new Vector3(0, 0, this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, out groundInfo, groundCheckDistance)
            //|| Physics.Raycast(this.gameObject.transform.position + new Vector3(0, 0, -this.gameObject.transform.localScale.z * groundCheckSize), Vector3.down, out groundInfo, groundCheckDistance)
            ) //out groundInfo
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
        if(playerState != PlayerState.stunned)
        {
            //reset state
            playerState = PlayerState.defaultMovement;
        }

    }

    public IEnumerator ReturnPlayerStateToMovingStun(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //reset state
        //TempFlatten(false);
        //unflatten by calling to idle
        playerAnimation.CallAnimTrigger("ToIdle");
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

    public void StunMe(Vector3 stunDirection, float stunLength)
    {
        if(playerState == PlayerState.stunned)
        {
            return;
        }

        playerState = PlayerState.stunned;
        //drop any held board before flattening
        if(playerActions.HeldPlank != null)
        {
            playerActions.SetUpAndExecuteAction(PlayerActions.PlayerActionType.drop);
        }

        //flatten
        //TempFlatten(true);

        //call flatten animation
        playerAnimation.StunnedAnim(stunDirection);

        StartCoroutine(ReturnPlayerStateToMovingStun(stunLength));
    }
    /*
    private void TempFlatten(bool flattenMe)
    {
        Vector3 temp;

        

        if (flattenMe)
        {
            print("flatten");
            //flattten
            //print("faltten");
            temp = this.gameObject.transform.localScale;
            temp.y *= flattenPercent;
            this.gameObject.transform.localScale = temp;
            //move down (OBSELETE)
            temp = this.gameObject.transform.position;
            //temp.y -= flattenDownAmount;
            this.gameObject.transform.position = temp;
        }
        else
        {
            print("unflatten");
            //move up (OBSELETE)
            temp = this.gameObject.transform.position;
            //temp.y += flattenDownAmount;
            this.gameObject.transform.position = temp;
            //un flatten
            //print("unflatten");
            temp = this.gameObject.transform.localScale;
            temp.y *= (1/flattenPercent);
            this.gameObject.transform.localScale = temp;
            
        }
    }
    */

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

    

}

