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

    public void PushAnim()
    {
        playerAnimator.SetTrigger("ToPush");
    }

}
