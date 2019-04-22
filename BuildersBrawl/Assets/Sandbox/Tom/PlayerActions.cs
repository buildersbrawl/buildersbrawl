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

    public float shit;

    public delegate void PlayerActionDelegate();
    PlayerActionDelegate playerActionDelegate;


    //player controller ref
    PlayerController playerController;

    public float actionCooldown = 1f;

    private Vector3 whereBoardHeld = new Vector3(0, 1.1f, -.5f);
    private float zRotationEnd = -160f;

    public bool throwBoardOnDrop = false;

    [Header("BoxCast")]
    //[SerializeField]
    private Vector3 boxCastOffset = new Vector3(0,0,0);
    //[SerializeField]
    private Vector3 boxCasthalfSize = new Vector3(.7f, 1.5f, .1f);
    //[SerializeField]
    private float boxCastMaxDistancePlankPickUp = 1;
    [SerializeField]
    private float boxCastMaxDistancePush = .5f;
    //[SerializeField]
    private float boxCastMaxDistanceSlam = 2;
    private Quaternion playerRotation;
    private Vector3 playerForward;

    RaycastHit[] boxHitInfo;

    //public bool holdingBoard = false;

    [Header("TestCubes")]
    private bool testCubes;
    private GameObject startCube;
    private GameObject endCube;

    [SerializeField]
    private GameObject heldPlank;
    public GameObject HeldPlank
    {
        get
        {
            return heldPlank;
        }
    }

    [Header("Board Slam")]
    [Tooltip("How long to wait after board slam called to check to see if there is a player infront of me")]
    [SerializeField]
    private float boardSlamDelayForAnim = .1f;
    [SerializeField]
    private float slamCooldown = 2f;
    [SerializeField]
    private float slamStopTime = .9f; //NOTE: SHOULD BE SHORTER THAN ASSOSSIATIED COOLDOWN

    [Header("Push")]
    [SerializeField]
    private float pushForce = 13f;
    [SerializeField]
    private float pushDelayForAnim = .15f;
    [SerializeField]
    private float pushCooldown = 1f;
    [SerializeField]
    private float pushedCooldown = 1f;
    [SerializeField]
    private float pushStopTime = .5f; //NOTE: SHOULD BE SHORTER THAN ASSOSSIATIED COOLDOWN

    
    [HideInInspector]
    public bool didNotFindPlank = false;
    [Header("PickUpPlace")]
    [SerializeField]
    private float pickPlaceDelayForAnim = .15f;
    [SerializeField]
    private float pickUpPlaceCooldown = .9f;
    [SerializeField]
    private float PickUpPlaceStopTime = .9f; //NOTE: SHOULD BE SHORTER THAN ASSOSSIATIED COOLDOWN

    //cooldowns
    private float coolDownLength;
    private float defaultCooldown = .1f;
    [Header("Stun")]
    [SerializeField]
    private float slamStunTime = 3f;

    private bool boardAnimCont = true;
    private float boardAnimationTime = 0;
    private bool boardAnimSwitch = true;

    //
    private float boardAnimSpeed = 10f;

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

        ActionAnimation(pAT);
        playerActionDelegate();

    }

    private void PreAction()
    {
        //print("PreAction");
        playerController.playerState = PlayerController.PlayerState.action;
        //cooldown set to default cooldown
        coolDownLength = defaultCooldown;
        
        //for picking up
        didNotFindPlank = false;
    }

    private void PostAction()
    {
        StartCoroutine(playerController.ReturnPlayerStateToMoving(coolDownLength));
        //print("PostAction");
    }

    //------------------------------------------------------------------------------------

    //calls appropriate animation
    private void ActionAnimation(PlayerActionType pAT)
    {
        switch (pAT)
        {
            case PlayerActionType.push:
                playerController.playerAnimation.CallAnimTrigger("ToPush");
                StartCoroutine(playerController.playerAnimation.TempPlayerStopForAnim(pushStopTime));
                break;
            case PlayerActionType.charge:
                playerController.playerAnimation.CallAnimTrigger("ToCharge");
                break;
            case PlayerActionType.pickUp:
                print("called pickup animation");
                playerController.playerAnimation.CallAnimTrigger("ToBoardPickUp");
                StartCoroutine(playerController.playerAnimation.TempPlayerStopForAnim(PickUpPlaceStopTime));
                break;
            case PlayerActionType.drop:
                //NA drop animations handled in "PushMe" and "StunMe" functions
                break;
            case PlayerActionType.slam:
                playerController.playerAnimation.CallAnimTrigger("ToSlam");
                StartCoroutine(playerController.playerAnimation.TempPlayerStopForAnim(slamStopTime));
                break;
            case PlayerActionType.place:
                playerController.playerAnimation.CallAnimTrigger("ToPlacingBoard");
                StartCoroutine(playerController.playerAnimation.TempPlayerStopForAnim(PickUpPlaceStopTime));
                break;
            default:
                break;
        }
    }

    //---------------------------------------------------------------------------------------
    //PUSH


    private void PushAction()
    {
        //cant do if holding plank
        if (heldPlank != null)
        {
            return;
        }

        //call coroutine
        StartCoroutine(PushCoroutine());
        

    }

    //add slight delay to push so that looks right with animation
    IEnumerator PushCoroutine()
    {
        yield return new WaitForSeconds(pushDelayForAnim);

        print("Push action");
        //make boxcast in front of player
        SeeWhatIsInFrontOfPlayer(boxCastMaxDistancePush);

        //if hits opponent knock back opponent
        for (int index = 0; index < boxHitInfo.Length; index++)
        {
            print("Push hit " + boxHitInfo[index]);

            //look to see if hit player other than self
            if (boxHitInfo[index].collider.gameObject.GetComponent<PlayerController>() != null && boxHitInfo[index].collider.gameObject != this.gameObject)
            {
                //"push" that player
                //get the vector this player is facing      //boxcasting sets player forward

                //push other player
                boxHitInfo[index].collider.GetComponent<PlayerController>().playerMovement.PushMe(playerForward, pushForce);

                //tell the other player it was I who pushed you
                boxHitInfo[index].collider.GetComponent<PlayerDeath>().OtherPlayer = this.gameObject;

                //make sure this player goes on cooldown
                coolDownLength = pushCooldown;

                //end loop
                index = boxHitInfo.Length;
            }
        }

        if (testCubes)
        {
            TestBoxCast(startCube, endCube, boxCastMaxDistancePush);
        }

    }

    //-------------------------------------------------------



    private void ChargeAction()
    {
        print("Charge action");
        //move player forward in direction
        //make boxcast in front of player
        //if hits opponent knock back opponent (probably more force than push)

    }

    private void PickUpPlankAction()
    {
        StartCoroutine(PickUpPlankCoroutine());

    }

    private IEnumerator PickUpPlankCoroutine()
    {
        yield return new WaitForSeconds(pickPlaceDelayForAnim);

        //pick cooldown
        coolDownLength = pickUpPlaceCooldown;

        //print("Try PickUpBoard action");

        //check to see if board is in front of player
        //boxcast in front of player


        //boxcast (makes an array)
        SeeWhatIsInFrontOfPlayer(boxCastMaxDistancePlankPickUp);

        //bool to determine whether animation goes back to idle
        didNotFindPlank = true;

        //go through array looking for plank
        for (int index = 0; index < boxHitInfo.Length; index++)
        {
            //if hits board that is dropped, pick up board
            //if hit plank, and plank is dropped
            if (boxHitInfo[index].collider.gameObject.GetComponent<PlankManager>() != null && boxHitInfo[index].collider.gameObject.GetComponent<PlankManager>().plankState == PlankManager.PlankState.dropped)
            {
                PickUpPlank(boxHitInfo[index].collider.gameObject);
                didNotFindPlank = false;

                //stop looking for other hit boards (stop for loop)
                index = boxHitInfo.Length;
            }

            //if hits board maker, make board, pick up board

            else if (boxHitInfo[index].collider.gameObject.GetComponent<PlankPile>() != null)
            {
                //make plank
                GameObject newPlank = boxHitInfo[index].collider.gameObject.GetComponent<PlankPile>().GeneratePlank(this.gameObject.transform.position, this.gameObject.transform.rotation);

                //pivk it up
                PickUpPlank(newPlank);
                didNotFindPlank = false;
                //stop looking (stop for loop)
                index = boxHitInfo.Length;
            }

            //----------------------------------

        }

        //make cubes to visualize boxcast
        if (testCubes)
        {
            TestBoxCast(startCube, endCube, boxCastMaxDistancePlankPickUp);
        }

        StartCoroutine(PickUpAnimDeterminer());

    }

    

    private void PickUpPlank(GameObject plank)
    {
        heldPlank = plank;

        //set state of plank
        //heldPlank.GetComponent<PlankManager>().plankState = PlankManager.PlankState.held;

        //make parent
        heldPlank.transform.parent = this.gameObject.transform;

        //rotate it so facing correct direction
        heldPlank.transform.rotation = this.gameObject.transform.rotation;
        //add 90 degrees to rotation
        heldPlank.transform.Rotate(new Vector3(0, 90, 0));

        //move up a bit so over players head
        heldPlank.transform.position = this.gameObject.transform.position + (this.transform.forward * 2) + new Vector3(0, -.4f, 0);

        //PICK UP
        heldPlank.GetComponent<PlankManager>().PickUpPlankCall(this.gameObject);
    }

    
    IEnumerator PickUpAnimDeterminer()
    {
        float determinerTime = pickUpPlaceCooldown;

        //if already in picked up state don't take time to determine outcome
        if (playerController.playerAnimation.InAnimState("boardPickUp"))
        {
            determinerTime = 0;
        }

        yield return new WaitForSeconds(determinerTime);
        //if trying to pick up plank and failed go back to idle animation
        if (didNotFindPlank)
        {
            playerController.playerAnimation.CallAnimTrigger("ToIdle");
        }
        else
        {
            playerController.playerAnimation.CallAnimTrigger("ToIdleBoard");
        }

    }
    

    private void PlacingPlankAction()
    {
        //place cooldown
        coolDownLength = pickUpPlaceCooldown;

        //print("placing board action");

        //if player holding a board
        if (heldPlank != null)
        {
            //unchild it
            //heldPlank.transform.parent = null;

            //set to placing
            heldPlank.GetComponent<PlankManager>().PlacingPlankCall();

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

        //cooldown should be default cooldown (0)

    }

    private void BoardSlamAction()
    {
        print("board slam action");

        if(heldPlank == null)
        {
            return;
        }
        //turn on player collider detection
        //heldPlank.GetComponent<PlankManager>().SetToHitPlayers();

        
        //plank animation
        StartCoroutine(BoardSlamPlankAnim());
     
        //board slam animation called earlier

        //after certain amount of time cast forward to see if player in front of me
        StartCoroutine(BoardSlamCoroutine());
        //if that player isn't me
        //stun them
        //flatten them
        //cooldown


        //turn off player collider detection
        heldPlank.GetComponent<PlankManager>().SetToNotHitPlayers();

    }

    private IEnumerator BoardSlamCoroutine()
    {
        //after certain amount of time cast forward to see if player in front of me (because slam animation has to happen)
        //if that player isn't me
        //stun them
        //flatten them
        //cooldown
        yield return new WaitForSeconds(boardSlamDelayForAnim);
        SeeWhatIsInFrontOfPlayer(boxCastMaxDistanceSlam);
        if (testCubes)
        {
            TestBoxCast(startCube, endCube, boxCastMaxDistanceSlam);
        }
        for (int index = 0; index < boxHitInfo.Length; index++)
        {
            //if that player isn't me
            if(boxHitInfo[index].collider.gameObject.GetComponent<PlayerController>() != null && !(boxHitInfo[index].collider.gameObject.Equals(this.gameObject)))
            {
                //stun that player
                PlayerController slammedPlayer = boxHitInfo[index].collider.gameObject.GetComponent<PlayerController>();
                slammedPlayer.StunMe(playerForward, slamStunTime);

                //make slam go on cooldown cause successfully hit someone
                coolDownLength = slamCooldown;
            }
        }

    }

    /*
    private IEnumerator TempPlankAnim()
    {
        boardAnimCont = true;
        boardAnimationTime = 0;
        boardAnimSwitch = true;
        //up 3 down 1

        float divideFactor = 50f;

        while (boardAnimCont)
        {
            yield return new WaitForSeconds(.01f);
            //rotate plank on z
            if (boardAnimSwitch)
            {
                //go up
                HeldPlank.transform.position += new Vector3(0, (-boardAnimSpeed * 2) / divideFactor, boardAnimSpeed / divideFactor);
                HeldPlank.gameObject.transform.localEulerAngles += Vector3.forward * boardAnimSpeed;
                //HeldPlank.gameObject.transform.localPosition += new Vector3(0, .1f, 0);
                //print("up");
            }
            else
            {
                //go down
                HeldPlank.transform.position += new Vector3(0, (boardAnimSpeed * 2) / divideFactor, -boardAnimSpeed / divideFactor);
                HeldPlank.gameObject.transform.localEulerAngles -= Vector3.forward * boardAnimSpeed;
                //HeldPlank.gameObject.transform.localPosition -= new Vector3(0,.1f,0);

            }
            boardAnimationTime += .2f;
            if (boardAnimationTime >= 1f)
            {
                boardAnimSwitch = false;
            }

            if (boardAnimationTime >= 2f)
            {
                boardAnimCont = false;
            }
            else
            {
                print("cont");
                //StartCoroutine(TempPlankAnim());
            }
        }
    }

    */

    //rotate 160 degrees
    //move down 3 forward 1
    //in 1 second

    //then rotate -160 degrees
    //move up 3 back 1
    //in 2 seconds

    IEnumerator BoardSlamPlankAnim()
    {
        print("TempAnim2");

        float boardDownTime = 1f;
        float boardUpTime = boardDownTime * 2f; //board slam
        float interpolationPercent = 0;
        float interpolationIncrementDown = .1f;
        float interpolationIncrementUp = .025f; //how much each hundreth of a second counts for

        Vector3 startPosition = heldPlank.transform.localPosition;
        Vector3 startRotation = heldPlank.transform.localEulerAngles;

        Vector3 endPosition = startPosition + new Vector3(0, -1.4f, 2.4f);
        Vector3 endRotation = heldPlank.transform.localEulerAngles + new Vector3(0, 0, 160);

        boardAnimCont = true;
        boardAnimSwitch = true;

        while (boardAnimCont)
        {
            yield return new WaitForSeconds(.01f);

            //rotate plank on z
            if (boardAnimSwitch)
            {
                interpolationPercent += interpolationIncrementDown;

                heldPlank.transform.localPosition = Vector3.Lerp(startPosition, endPosition, interpolationPercent);
                heldPlank.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, interpolationPercent);
            }
            else
            {
                interpolationPercent += interpolationIncrementUp;

                heldPlank.transform.localPosition = Vector3.Lerp(endPosition, startPosition, interpolationPercent);
                heldPlank.transform.localEulerAngles = Vector3.Lerp(endRotation, startRotation, interpolationPercent);
            }

            //boardAnimationTime += .01f;

            //determines switch and end
            if (boardAnimSwitch && (interpolationPercent >= 1f))
            {
                boardAnimSwitch = false;
                interpolationPercent = 0;
                print("switch");
            }
            else if (!boardAnimSwitch && (interpolationPercent >= 1f))
            {
                boardAnimCont = false;
                print("end");
            }
        }
    }




    public void TestBoxCast(GameObject startCube, GameObject endCube, float maxDistance)
    {
        //print("testbox");

        startCube.GetComponent<Collider>().enabled = false;
        startCube.transform.position = this.gameObject.transform.position + boxCastOffset;
        startCube.transform.rotation = playerRotation;
        startCube.transform.localScale = boxCasthalfSize * 2;

        endCube.GetComponent<Collider>().enabled = false;
        endCube.transform.position = this.gameObject.transform.position + boxCastOffset + (playerForward * maxDistance);
        endCube.transform.rotation = playerRotation;
        endCube.transform.localScale = boxCasthalfSize * 2;
    }

    //boxcast
    public void SeeWhatIsInFrontOfPlayer(float maxDistance)
    {
        playerRotation = this.gameObject.transform.rotation;
        playerForward = this.gameObject.transform.forward;

        boxHitInfo = Physics.BoxCastAll(this.transform.position + boxCastOffset, boxCasthalfSize, playerForward, playerRotation, maxDistance);
    }
}

