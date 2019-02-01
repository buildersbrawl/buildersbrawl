using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //player controller ref
    PlayerController playerController;

    public float playerSpeed = 1f;
    [Tooltip("What percentage the player slows down when jumping/holding a plank")]
    public float slowDownSpeed = .5f;

    Vector3 playerFinalDirection; 

    public float jumpTime = 1f;

    public float jumpHeight = 1f;
    private Vector3 jumpVector;

    public void InitMove(PlayerController pC)
    {
        playerController = pC;
        jumpVector = new Vector3(0, jumpHeight, 0);
    }

    public Vector3 PlayerSideMovement(Vector3 direction, PlayerController.PlayerState pState)
    {
        playerFinalDirection = direction * playerSpeed;

        //if player jumping or holding plank slow them down
        if(playerController.playerState == PlayerController.PlayerState.jumping || playerController.playerState == PlayerController.PlayerState.holdingPlank)
        {
            playerFinalDirection *= slowDownSpeed;
        }
        return playerFinalDirection;
    }

    public Vector3 Jump(bool buttonPressed)
    {
        if (buttonPressed)
        {
            //TEMP
            //TODO: IN FUTURE MAKE IT SO JUMP RESETS WHEN PLAYER HITS THE GROUND
            //called to stop other unwanted ongoing movement
            playerController.playerState = PlayerController.PlayerState.jumping;
            StartCoroutine(playerController.ReturnPlayerStateToMoving(jumpTime));
            
            //jump
            return jumpVector;
        }
        else
        {
            return Vector3.zero;
        }
    }

}
