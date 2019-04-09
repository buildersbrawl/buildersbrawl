using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerController playerController;
    public Animator playerAnimator;

    //[SerializeField]
    private float minimumMovementForRun = .6f;

    //called in playerController init
    public void Init()
    {
        if (this.gameObject.GetComponent<PlayerController>() != null)
        {
            playerController = this.gameObject.GetComponent<PlayerController>();
        }
        else
        {
            playerController = this.gameObject.AddComponent<PlayerController>();
        }

        if(playerAnimator == null)
        {
            playerAnimator = this.GetComponentInChildren<Animator>();
        }
    }

    /*
    public void Animate(float waitTime, string animName)
    {
        StartCoroutine(WaitToAnim(waitTime, animName));
    }

    IEnumerator WaitToAnim(float waitTime, string animName)
    {
        yield return new WaitForSeconds(waitTime);
        playerAnimator.Play(animName);
    }
    */

    //calls appropriate animation based off of action
    public void ActionAnim(string toActionTransitionName)
    {
        if(playerAnimator == null)
        {
            print("no animator");
            return;
        }

        playerAnimator.SetTrigger(toActionTransitionName);
    }

    //determines what run animation to play
    //called in playerCOntroller
    public void RunAnim(Vector3 movement)
    {

        if(playerAnimator == null)
        {
            print("no animator");
            return;
        }

        if (SeeIfOtherTriggersActive())
        {
            return;
        }

        /*
        int runId = Animator.StringToHash("run");
        int idleId = Animator.StringToHash("idle");
        int idleBoardId = Animator.StringToHash("idleBoard");
        int runBoardId = Animator.StringToHash("runBoard");
        */

        string runId = "run";
        string idleId = "idle";
        string idleBoardId = "idleBoard";
        string runBoardId = "runBoard";

        AnimatorStateInfo currAnimStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        /*
        print("idle " + currAnimStateInfo.IsName("idle"));
        print("run " + currAnimStateInfo.IsName("run"));
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerAnimator.SetTrigger("ToRun");
            print("RUN!!!");
        }
        */
        //if going fast enough and not already running
        if (movement.magnitude >= minimumMovementForRun && (!(InSpecificStateOrTransition(runId) || (InSpecificStateOrTransition(runBoardId)))))
        {
            print("should run");

            //call appropriate run animation based off of whether holding a plank or not
            if ((currAnimStateInfo.IsName(idleId)))
            {
                //run
                playerAnimator.SetTrigger("ToRun");
                print("run anim");
            }
            else if ((currAnimStateInfo.IsName(idleBoardId)))
            {
                //runBoard
                playerAnimator.SetTrigger("ToRunBoard");
                print("runBoard anim");
            }
        }
        //if not going fast enough and not already idle
        else if (movement.magnitude < minimumMovementForRun && (!(InSpecificStateOrTransition(idleId) || (InSpecificStateOrTransition(idleBoardId)))))
        {
            print("should idle");
            //call appropriate idle animation based off of whether holding a plank or not
            if ((currAnimStateInfo.IsName(runId)))
            {
                //idle
                playerAnimator.SetTrigger("ToIdle");
                print("idle anim");
            }
            else if ((currAnimStateInfo.IsName(runBoardId)))
            {
                //idleBoard
                playerAnimator.SetTrigger("ToIdleBoard");
                //print("idleBoard anim: " + playerAnimator.GetNextAnimatorStateInfo(0).IsName("idle"));
            }
        }
        

    }

    private bool InSpecificStateOrTransition(string stateName)
    {
        bool inState = false;

        AnimatorStateInfo currAnimStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        //true if in transition
        inState = playerAnimator.IsInTransition(0);

        //true if in appropriate state
        if (currAnimStateInfo.IsName(stateName))
        {
            inState = true;
        }

        return inState;
    }

    //for when player is pushed
    public void PushedAnim(Vector3 pushedFromDirection)
    {
        if (playerAnimator == null)
        {
            print("no animator");
            return;
        }

        //calculate wheter or not pushed from front or back

        //see if players current looking direction is less than 90 (oblique) then push from back
        if(Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, pushedFromDirection)) < 90)
        {
            playerAnimator.SetTrigger("ToPushedBack");
        }
        else
        {
            //else push from front
            playerAnimator.SetTrigger("ToPushedFront");
        }
    }

    public void StunnedAnim(Vector3 stunnedFromDirection)
    {
        if (playerAnimator == null)
        {
            print("no animator");
            return;
        }

        //calculate wheter or not slammed from front or back

        //see if players current looking direction is less than 90 (oblique) then push from back
        if (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, stunnedFromDirection)) < 90)
        {
            playerAnimator.SetTrigger("ToSquashBack");
        }
        else
        {
            //else push from front
            playerAnimator.SetTrigger("ToSquashFront");
        }
    }

    /*
    public void ChargedAnim()
    {
        if (playerAnimator == null)
        {
            print("no animator");
            return;
        }

        playerAnimator.SetTrigger("ToFrontImpact");
    }
    

    public void DropAnim()
    {
        if (playerAnimator == null)
        {
            print("no animator");
            return;
        }


    }
    */

    //see if any other triggers active
    public bool SeeIfOtherTriggersActive()
    {
        bool otherTriggersActive = false;

        int activeParameters = 0;

        for (int index = 0; index < playerAnimator.parameterCount; index++)
        {
            //if other trigger active add one to activeparameters
            if (playerAnimator.GetBool(playerAnimator.GetParameter(index).name) == true)
            {
                activeParameters++;
            }
        }

        if(activeParameters > 0)
        {
            otherTriggersActive = true;
        }

        return otherTriggersActive;
    }



}
