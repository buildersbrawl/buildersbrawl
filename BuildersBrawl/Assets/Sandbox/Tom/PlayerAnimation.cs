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

    public void PushedAnim()
    {
        if (playerAnimator == null)
        {
            print("no animator");
            return;
        }

        //calculate wheter or not pushed from front or back

    }

    public void StunnedAnim()
    {
        if (playerAnimator == null)
        {
            print("no animator");
            return;
        }

        //calculate wheter or not slammed from front or back

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
