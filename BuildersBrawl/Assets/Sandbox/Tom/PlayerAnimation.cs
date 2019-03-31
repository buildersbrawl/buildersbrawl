using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerController playerController;
    public Animator playerAnimator;

    [SerializeField]
    private float minimumMovementForRun = .1f;

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

        //if going fast enough and not already running
        if(movement.magnitude >= minimumMovementForRun && (!(playerAnimator.GetNextAnimatorStateInfo(0).IsName("run") || playerAnimator.GetNextAnimatorStateInfo(0).IsName("runBoard"))))
        {
            
            //call appropriate run animation based off of whether holding a plank or not
            if(playerAnimator.GetNextAnimatorStateInfo(0).IsName("Brawl_Idle_Final"))
            {
                //run
                playerAnimator.SetTrigger("ToRun");
                print("run anim");
            }
            else 
            {
                //runBoard
                playerAnimator.SetTrigger("ToRunBoard");
                print("runBoard anim");
            }
        }
        else if (movement.magnitude < minimumMovementForRun && (!(playerAnimator.GetNextAnimatorStateInfo(0).IsName("Brawl_Idle_Final") || playerAnimator.GetNextAnimatorStateInfo(0).IsName("idleBoard"))))
        {
            
            //call appropriate idle animation based off of whether holding a plank or not
            if (playerAnimator.GetNextAnimatorStateInfo(0).IsName("Brawl_Idle_Final"))
            {
                //idle
                playerAnimator.SetTrigger("ToIdle");
                print("Brawl_Idle_Final anim");
            }
            else
            {
                //idleBoard
                playerAnimator.SetTrigger("ToIdleBoard");
                print("idleBoard anim: " + playerAnimator.GetNextAnimatorStateInfo(0).IsName("Brawl_Idle_Final"));
            }
        }
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

        //see if players current looking direction is greater than 90 (oblique) then push from back
        if(Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, pushedFromDirection)) > 90)
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

        //see if players current looking direction is greater than 90 (oblique) then push from back
        if (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, stunnedFromDirection)) > 90)
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



}
