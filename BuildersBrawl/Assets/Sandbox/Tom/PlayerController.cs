using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        defaultMovement,
        jumping,
        action,
        holdingPlank,
        cooldown
    }

    public PlayerState playerState;

    //Char cont reference
    CharacterController charContRef;

    //player actions ref
    PlayerActions playerActions;

    //player move ref
    PlayerMovement playerMovement;

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


    [Header("For Testing")]
    public bool right;
    public bool left;
    public bool forward;
    public bool backwards;
    public bool AInput;
    public bool BInput;
    public bool XInput;
    public bool YInput;
    public bool BumpOrTrigInput;

    Vector3 joyInput;

    [Header("Bounce")]
    public float bounciness = 1f;
    [SerializeField]
    private float collisonCheckLength;
    [Tooltip("The more drag the faster the players excess momentum is stopped. Value between 0-1.")]
    [SerializeField]
    private float drag = .2f;

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

    //private float slowDownPhysics = 1f;

    //around cos(pi/4) or sin (pi/4) (they're the same number)
    private const float TURN_INPUT_45_DEGREES_CONSTANT = 0.70710678118f;

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


        playerState = PlayerState.defaultMovement;

        //test
        joyInput = Vector3.zero;

    }

    //unsteady frames for input
    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            right = true;
        }
        else
        {
            right = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            left = true;
        }
        else
        {
            left = false;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            forward = true;
        }
        else
        {
            forward = false;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            backwards = true;
        }
        else
        {
            backwards = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AInput = true;
        }
        else
        {
            AInput = false;
        }

        if (right)
        {
            joyInput += ConvertJoystickInputToNewDirection(new Vector3(1, 0, 0));
        }
        if (left)
        {
            joyInput += ConvertJoystickInputToNewDirection(new Vector3(-1, 0, 0));
        }
        if (forward)
        {
            joyInput += ConvertJoystickInputToNewDirection(new Vector3(0, 0, 1));
        }
        if (backwards)
        {
            joyInput += ConvertJoystickInputToNewDirection(new Vector3(0, 0, -1));
        }

    }

    //steady frames for physics and movement
    public void FixedUpdate()
    {

        //check to see if just hit the ground from jumping
        if (playerGrounded && playerState == PlayerState.jumping)
        {
            playerState = PlayerState.cooldown;
            //start jump cooldown
            StartCoroutine(ReturnPlayerStateToMoving(playerMovement.postJumpCooldown));
        }

        //if not doing something else and grounded a will activate jump
        if (playerState != PlayerState.action && playerState != PlayerState.jumping && playerState != PlayerState.cooldown && playerState != PlayerState.holdingPlank && playerGrounded)
        {
            //a jump
            moveVector += playerMovement.Jump(AInput);
        }

        //print("After jump state is " + playerState);

        //IF JUMPING OVERRIDE ACTION
        if (playerState != PlayerState.jumping && playerState != PlayerState.cooldown)
        {
            //x is push

            //b is charge

            //y pick up
            //y drop board

            //bumpers are board slam
        }

        //move based off of joystick, if performing action don't do this
        if (playerState != PlayerState.action)
        {
            //call player movement based off of joystick movement
            moveVector += playerMovement.PlayerSideMovement(joyInput, playerState);
        }

        //apply gravity
        moveVector += playerMovement.Gravity() * Time.fixedDeltaTime;

        //print("After update state is " + playerState);

        //print(moveVector);

        //apply movement
        charContRef.Move(moveVector * Time.fixedDeltaTime);

        //next frame figure out collision
        moveVector += CollisionCheck(moveVector);

        //see if grounded
        playerGrounded = IsPlayerGrounded();

        //print("Groundid is: " + playerGrounded);

        //before reset record last vector
        lastFramesMoveVector = moveVector;

        //reset moveVector by getting rid of impact of joystick input
        moveVector += playerMovement.reversePlayerMovementFromJoysticks;
        //now if there extra x/z movement (momentum) reduce it with drag
        moveVector = ApplyDrag(moveVector);

        //old way
        //moveVector.x = 0;
        //moveVector.z = 0;

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

    public bool IsPlayerGrounded()
    {
        //raycast down short distance, if hits anything grounded
        //Debug.DrawRay(this.gameObject.transform.position, Vector3.down, Color.red, groundCheckDistance);
        if(Physics.Raycast(this.gameObject.transform.position, Vector3.down, groundCheckDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
        //return charContRef.isGrounded;
    }

    public IEnumerator ReturnPlayerStateToMoving(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //reset state
        playerState = PlayerState.defaultMovement;
    }

    //takes input and rotates it 45 degrees to match the 
    public Vector3 ConvertJoystickInputToNewDirection(Vector3 input)
    {
        Vector3 answer = Vector3.zero;

        //used these equations to rotate things on a 2d plane. Thank you stack overflow: https://stackoverflow.com/questions/14607640/rotating-a-vector-in-3d-space
        //newX = x cos θ − y sin θ
        //newY = x sin θ + y cos θ

        answer.x = (input.x * TURN_INPUT_45_DEGREES_CONSTANT) - (input.z * TURN_INPUT_45_DEGREES_CONSTANT);
        answer.z = (input.x * TURN_INPUT_45_DEGREES_CONSTANT) + (input.z * TURN_INPUT_45_DEGREES_CONSTANT);

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


    public Vector3 ApplyDrag(Vector3 directionMoving)
    {
        //take x and z
        //reduce them by drag percentage
        directionMoving.x *= (1 - drag);
        directionMoving.z *= (1 - drag);

        return directionMoving;
    }

}
