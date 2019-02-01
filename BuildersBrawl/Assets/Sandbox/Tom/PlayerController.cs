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
        holdingPlank
    }

    public PlayerState playerState;

    //Char cont reference
    CharacterController charContRef;

    //player actions ref
    PlayerActions playerActions;

    //player move ref
    PlayerMovement playerMovement;

    //determines player movement
    Vector3 moveVector;

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
        if (right)
        {
            joyInput += new Vector3(1, 0, 0);
        }
        if (left)
        {
            joyInput += new Vector3(-1, 0, 0);
        }
        if (forward)
        {
            joyInput += new Vector3(0, 0, 1);
        }
        if (backwards)
        {
            joyInput += new Vector3(0, 0, -1);
        }

    }

    //steady frames for physics and movement
    public void FixedUpdate()
    {
        
        if (playerState != PlayerState.action && playerState != PlayerState.jumping)
        {
            //a jump
            moveVector += playerMovement.Jump(AInput);
        }


        //IF JUMPING OVERRIDE ACTION
        if (playerState != PlayerState.jumping && playerState != PlayerState.holdingPlank)
        {
            //x is push

            //b is charge

            //y pick up
            //y drop board

            //bumpers are board slam
        }

        //IF PERFORMING ACTION OVERRIDE JOYSTICK
        if (playerState != PlayerState.action && playerState != PlayerState.holdingPlank)
        {
            //call player movement based off of joystick movement
            moveVector += playerMovement.PlayerSideMovement(joyInput, playerState);
        }

        print(moveVector);

        //apply movement
        charContRef.Move(moveVector);
        //reset moveVector
        moveVector = Vector3.zero;
        joyInput = Vector3.zero;
    }

    public IEnumerator ReturnPlayerStateToMoving(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //reset state
        playerState = PlayerState.defaultMovement;
    }

}
