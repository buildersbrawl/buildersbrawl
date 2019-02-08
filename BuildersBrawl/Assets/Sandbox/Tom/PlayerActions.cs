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

    public void Push()
    {

    }

    public void Charge()
    {

    }

    public void PickUpBoard()
    {
        //check to see if board is in front of player
        //boxcast in front of player
        //Physics.BoxCast()

        //if hits board that is dropped, pick up board
        //if hits board maker, make board, pick up board

        //make board child of player, put above players head


    }

    public void PlaceBoard()
    {
        //if player holding a board
        //unchild it
        //rotate it so facing correct direction
        //set to placing
    }

    public void DropBoard()
    {
        //if player holding a board
        //unchild it
        //set to dropped

    }

    public void BoardSlam()
    {

    }



}
