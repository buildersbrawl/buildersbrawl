using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    //player controller ref
    PlayerController playerController;

    public float actionCooldown = 1f;

    public void InitAct(PlayerController pC)
    {
        playerController = pC;
    }

    public void PlayerActionsFunction()
    {
        //x is push

        //b is charge

        //y pick up and drop board

        //triggers is board slam
    }
}
