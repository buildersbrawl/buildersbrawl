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
        playerAnimator.SetTrigger(toActionTransitionName);
    }

    public void PushedAnim()
    {

    }

    public void StunnedAnim()
    {

    }

    public void DropAnim()
    {

    }

}
