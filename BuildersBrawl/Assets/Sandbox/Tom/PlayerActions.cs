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
        slam,
        place
    }

    public delegate void PlayerActionDelegate();
    PlayerActionDelegate playerActionDelegate;

    //player controller ref
    PlayerController playerController;

    public float actionCooldown = 1f;

    public Vector3 whereBoardHeld = new Vector3(0, 1.5f, 0);

    public bool throwBoardOnDrop = false;

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

    //public bool holdingBoard = false;

    [Header("TestCubes")]
    public bool testCubes;
    public GameObject startCube;
    public GameObject endCube;

    [SerializeField]
    private GameObject heldPlank;
    public GameObject HeldPlank
    {
        get
        {
            return heldPlank;
        }
    }

    private bool omitCooldown = false;

    [Header("Push")]
    public float pushForce = 1f;

    //------------------------------------------------------------------------------------------------------

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
            case PlayerActionType.place:
                playerActionDelegate = null;
                playerActionDelegate += PreAction;
                playerActionDelegate += PlacingPlankAction;
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
        omitCooldown = false;
        //stop outside movement? Weird movement caused by it subtracting last frames movement constatntly?
    }

    private void PostAction()
    {
        if (omitCooldown)
        {
            playerController.playerState = PlayerController.PlayerState.defaultMovement;
            //reset
            omitCooldown = false;
        }
        else
        {
            StartCoroutine(playerController.ReturnPlayerStateToMoving(actionCooldown));
        }
        //print("PostAction");
        
    }


    private void PushAction()
    {
        //cant do if holding plank
        if(heldPlank != null)
        {
            return;
        }

        print("Push action");
        //make boxcast in front of player
        SeeWhatIsInFrontOfPlayer();
        //if hits opponent knock back opponent
        for (int index = 0; index < boxHitInfo.Length; index++)
        {
            print("Push hit " + boxHitInfo[index]);

            //look to see if hit player other than self
            if(boxHitInfo[index].collider.gameObject.GetComponent<PlayerController>() != null && boxHitInfo[index].collider.gameObject != this.gameObject)
            {
                //"push" that player
                //get the vector this player is facing      //boxcasting sets player forward

                //push other player
                boxHitInfo[index].collider.GetComponent<PlayerController>().PushMe(playerForward, pushForce);

                //end loop
                index = boxHitInfo.Length;
            }
        }
    }

    private void ChargeAction()
    {
        print("Charge action");
        //move player forward in direction
        //make boxcast in front of player
        //if hits opponent knock back opponent (probably more force than push)

    }

    private void PickUpPlankAction()
    {
        //print("Try PickUpBoard action");

        //check to see if board is in front of player
        //boxcast in front of player

        //boxcast (makes an array)
        SeeWhatIsInFrontOfPlayer();

        //cooldown not happening until proven otherwise
        omitCooldown = true;

        //go through array looking for plank
        for (int index = 0; index < boxHitInfo.Length; index++)
        {
            //if hits board that is dropped, pick up board
            //if hit plank, and plank is dropped
            if (boxHitInfo[index].collider.gameObject.GetComponent<PlankManager>() != null && boxHitInfo[index].collider.gameObject.GetComponent<PlankManager>().plankState == PlankManager.PlankState.dropped) 
            {
                PickUpPlank(boxHitInfo[index].collider.gameObject);

                //stop looking for other hit boards (stop for loop)
                index = boxHitInfo.Length;
            }

            //if hits board maker, make board, pick up board

            else if(boxHitInfo[index].collider.gameObject.GetComponent<PlankPile>() != null)
            {
                //make plank
                GameObject newPlank = boxHitInfo[index].collider.gameObject.GetComponent<PlankPile>().GeneratePlank(this.gameObject.transform.position, this.gameObject.transform.rotation);

                //pivk it up
                PickUpPlank(newPlank);

                //stop looking (stop for loop)
                index = boxHitInfo.Length;
            }

            //----------------------------------

        }

        //make cubes to visualize boxcast
        if (testCubes)
        {
            TestBoxCast(startCube, endCube);
        }

    }

    private void PickUpPlank(GameObject plank)
    {
        //cooldown is occuring
        omitCooldown = false;

        heldPlank = plank;

        //set state of plank
        //heldPlank.GetComponent<PlankManager>().plankState = PlankManager.PlankState.held;

        //PICK UP
        heldPlank.GetComponent<PlankManager>().PickUpPlank(this.gameObject);

        //make parent
        heldPlank.transform.parent = this.gameObject.transform;

        //rotate it so facing correct direction
        heldPlank.transform.rotation = this.gameObject.transform.rotation;
        //add 90 degrees to rotation
        heldPlank.transform.Rotate(new Vector3(0, 90, -5));

        //move up a bit so over players head
        heldPlank.transform.position = this.gameObject.transform.position + whereBoardHeld + this.gameObject.transform.forward;

    }

    private void PlacingPlankAction()
    {
        //print("placing board action");

        //if player holding a board
        if(heldPlank != null)
        {
            //unchild it
            heldPlank.transform.parent = null;

            //set to placing
            heldPlank.GetComponent<PlankManager>().PlacingPlank();

            //null heldplank
            heldPlank = null;
        }


    }

    private void DropPlankAction()
    {
        print("drop Plank action");

        //if player holding a board
        if (heldPlank != null)
        {
            //holding plank

            //unchild it
            heldPlank.transform.parent = null;

            heldPlank.GetComponent<PlankManager>().DropPlank();

            //fun
            if (throwBoardOnDrop)
            {
                print("throwing");
                heldPlank.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * 500f);
            }


            //get rid of reference to board
            heldPlank = null;

            //tell player they dropped board
            //holdingBoard = false;
        }

        //omit cooldown cause not players fault
        omitCooldown = true;

    }

    private void BoardSlamAction()
    {
        print("board slam action");

        if(heldPlank == null)
        {
            return;
        }
        //turn on player collider detection
        heldPlank.GetComponent<PlankManager>().SetToHitPlayers();
        //if hit opponent player

        //do board slam


        //turn off player collider detection
        heldPlank.GetComponent<PlankManager>().SetToNotHitPlayers();

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

    public void PlayActionAnimation()
    {
        //too put in delegate
    }

    //boxcast
    public void SeeWhatIsInFrontOfPlayer()
    {
        playerRotation = this.gameObject.transform.rotation;
        playerForward = this.gameObject.transform.forward;

        boxHitInfo = Physics.BoxCastAll(this.transform.position + boxCastOffset, boxCasthalfSize, playerForward, playerRotation, boxCastMaxDistance);
    }
}

