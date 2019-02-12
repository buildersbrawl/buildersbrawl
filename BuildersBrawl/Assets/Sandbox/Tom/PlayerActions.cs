using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    //EVERY TIME PLAYER PERFORMS ACTION THE INPUT IS RECTIFIED

    public enum PlayerActionType
    {
        push,
        charge,
        pickUp,
        drop,
        slam
    }

    public delegate void PlayerActionDelegate();
    PlayerActionDelegate playerActionDelegate;

    //player controller ref
    PlayerController playerController;

    public float actionCooldown = 1f;

    [Header("BoxCast")]
    [SerializeField]
    private Vector3 boxCastOffset = new Vector3(0,0,0);
    [SerializeField]
    private Vector3 boxCasthalfSize = new Vector3(.5f, .5f, .1f);
    [SerializeField]
    private float boxCastMaxDistance = 1;
    private Quaternion playerRotation;
    private Vector3 playerForward;

    RaycastHit[] boxHitInfo;

    public bool holdingBoard = false;

    [Header("TestCubes")]
    public bool testCubes;
    public GameObject startCube;
    public GameObject endCube;

    private GameObject heldPlank;

    public void InitAct(PlayerController pC)
    {
        playerController = pC;
    }

    public void SetUpAndExecuteAction(PlayerActionType pAT)
    {
        switch (pAT)
        {
            case PlayerActionType.push:
                playerActionDelegate = null;
                playerActionDelegate += PreAction;
                playerActionDelegate += PushAction;
                playerActionDelegate += PostAction;
                break;
            case PlayerActionType.charge:
                playerActionDelegate = null;
                playerActionDelegate += PreAction;
                playerActionDelegate += ChargeAction;
                playerActionDelegate += PostAction;
                break;
            case PlayerActionType.pickUp:
                playerActionDelegate = null;
                playerActionDelegate += PreAction;
                playerActionDelegate += PickUpPlankAction;
                playerActionDelegate += PostAction;
                break;
            case PlayerActionType.drop:
                playerActionDelegate = null;
                playerActionDelegate += PreAction;
                playerActionDelegate += DropPlankAction;
                playerActionDelegate += PostAction;
                break;
            case PlayerActionType.slam:
                playerActionDelegate = null;
                playerActionDelegate += PreAction;
                playerActionDelegate += BoardSlamAction;
                playerActionDelegate += PostAction;
                break;
            default:
                break;
        }

        playerActionDelegate();

    }

    private void PreAction()
    {
        //print("PreAction");
        playerController.playerState = PlayerController.PlayerState.action;

        //stop outside movement? Weird movement caused by it subtracting last frames movement constatntly?
    }

    private void PostAction()
    {
        //print("PostAction");
        StartCoroutine(playerController.ReturnPlayerStateToMoving(actionCooldown));
    }


    private void PushAction()
    {
        print("Push");

        
    }

    private void ChargeAction()
    {
        print("Charge");

        
    }

    private void PickUpPlankAction()
    {
        print("Try PickUpBoard");

        //check to see if board is in front of player
        //boxcast in front of player

        //make cubes to visualize cast
        //instantiate cubes

        playerRotation = this.gameObject.transform.rotation;
        playerForward = this.transform.forward;

        if (testCubes)
        {
            TestBoxCast(startCube, endCube);
        }


        //boxcast (is an array)
        boxHitInfo = Physics.BoxCastAll(this.transform.position + boxCastOffset, boxCasthalfSize, playerForward, playerRotation, boxCastMaxDistance);

        //go through array looking for plank
        for (int index = 0; index < boxHitInfo.Length; index++)
        {
            //if hits board that is dropped, pick up board
            //if hit plank, and plank is dropped
            if (boxHitInfo[index].collider.gameObject.GetComponent<PlankManager>() != null && boxHitInfo[index].collider.gameObject.GetComponent<PlankManager>().plankState == PlankManager.PlankState.dropped) 
            {
                heldPlank = boxHitInfo[index].collider.gameObject;

                //set state of plank
                heldPlank.GetComponent<PlankManager>().plankState = PlankManager.PlankState.held;

                heldPlank.GetComponent<PlankManager>().PickUpPlank();

                //make parent
                heldPlank.transform.parent = this.gameObject.transform;



                //move up a bit
                heldPlank.transform.position = this.gameObject.transform.position + Vector3.up;


                print("player picked up plank");

                holdingBoard = true;

                //stop looking for other hit boards
                index = boxHitInfo.Length;
            }

            //if hits board maker, make board, pick up board

            //----------------------------------

        }

        //make board child of player, put above players head

    }

    private void PlacePlankAction()
    {
        print("place board");

        //if player holding a board
        //unchild it
        //rotate it so facing correct direction
        //set to placing

    }

    private void DropPlankAction()
    {
        print("drop Plank");

        //if player holding a board
        if (heldPlank != null)
        {
            //holding plank

            //SHOULD HAPPEN IN PLANK
            //---------------------------------------------------------------
            //unchild it
            heldPlank.transform.parent = null;

            //set to dropped
            heldPlank.GetComponent<PlankManager>().plankState = PlankManager.PlankState.dropped;

            //give it physics
            heldPlank.GetComponent<Collider>().enabled = true;

            //add rigidbody
            if (heldPlank.GetComponent<Rigidbody>() == null)
            {
                heldPlank.AddComponent<Rigidbody>();
            }

            //---------------------------------------------------------------

            //get rid of reference to board
            heldPlank = null;

            //tell player they dropped board
            holdingBoard = false;
        }

    }

    private void BoardSlamAction()
    {
        print("board slam");

        //do stuff

    }

    public void TestBoxCast(GameObject startCube, GameObject endCube)
    {
        startCube.GetComponent<Collider>().enabled = false;
        startCube.transform.position = this.gameObject.transform.position + boxCastOffset;
        startCube.transform.rotation = playerRotation;
        startCube.transform.localScale = boxCasthalfSize * 2;

        endCube.GetComponent<Collider>().enabled = false;
        endCube.transform.position = this.gameObject.transform.position + boxCastOffset + (playerForward * boxCastMaxDistance);
        endCube.transform.rotation = playerRotation;
        endCube.transform.localScale = boxCasthalfSize * 2;
    }
}

