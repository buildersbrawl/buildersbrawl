using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerController playerController;
    public Animator playerAnimator;

    private void Start()
    {
        Init();
    }

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

    public void Animate(float waitTime, string animName)
    {
        StartCoroutine(WaitToAnim(waitTime, animName));
    }

    IEnumerator WaitToAnim(float waitTime, string animName)
    {
        yield return new WaitForSeconds(waitTime);
        playerAnimator.Play(animName);
    }

    public void ActionAnim(string toActionTransitionName)
    {
        if(playerAnimator == null)
        {
            print("no animator");
            return;
        }

        playerAnimator.SetTrigger(toActionTransitionName);
    }

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
            playerAnimator.SetTrigger("ToPushedBack");
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

}
