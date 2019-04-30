using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlankManager : MonoBehaviour
{

    //when a placing plank hits a non-plank or a plank that is not placed
    //make it drop
    //when a placing plank hits a placed plank
    //make it snap and place it


    //defulat is droppng
    public enum PlankState
    {
        dropped,
        beingplaced,
        held,
        placed,
        spawned
    }

    public PlankState plankState;

    private SnapTest2 snapRef;

    private PlayerController playerWhoPlacedMe;

    private PlankAnim plankAnim;

    bool initialized;

    private float timePlankCreated;

    //------------------------------------------------------------------------------------------------------

    private void Start()
    {
        timePlankCreated = Time.time;

        if (plankState == PlankState.dropped || plankState == PlankState.placed)
        {
            Init();
        }
    }

    public void PickUpSpawn()
    {
        //print("pick up");
        plankState = PlankState.spawned;
        //add this to game manager list
        GameManager.S.planksInScene.Add(this);
        Init();
    }

    public void Init()
    {
        //get snap
        if(this.gameObject.GetComponent<SnapTest2>() != null)
        {
            snapRef = this.gameObject.GetComponent<SnapTest2>();
        }
        else
        {
            snapRef = this.gameObject.AddComponent<SnapTest2>();
        }

        //get anim
        if (this.gameObject.GetComponent<PlankAnim>() != null)
        {
            plankAnim = this.gameObject.GetComponent<PlankAnim>();
        }
        else
        {
            plankAnim = this.gameObject.AddComponent<PlankAnim>();
        }
        plankAnim.SetPlankManager(this);

        //not trigger
        this.gameObject.GetComponent<Collider>().isTrigger = false;

        //default is dropping
        if (plankState == PlankState.beingplaced)
        {
            PlacingPlankCall();
        }
        else if (plankState == PlankState.held)
        {
            //PickUpPlankCall(playerWhoPlacedMe.gameObject);
            print("somethign wroong: initialized as held");
        }
        else if (plankState == PlankState.dropped)
        {
            DropPlank();
        }
        else if (plankState == PlankState.placed)
        {
            PlacePlank();
        }

    }


    public void PlacingPlankCall()
    {
        print(this.gameObject + " Placing");

        //don't let plank hit players
        SetToNotHitPlayers();

        //reduce size of collider
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.308f, 1, 1);
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(0.345f, 0, 0);

        //animation
        StartCoroutine(plankAnim.PutDownPlankAnim());
    }

    public void PlacingPlank()
    {

        //turn on collider
        this.gameObject.GetComponent<Collider>().enabled = true;

        //make trigger
        this.gameObject.GetComponent<Collider>().isTrigger = true;

        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //turn on artificial gravity
        snapRef.GravitySwitch(true);

        //make rotation -5 on z axis
        Vector3 tempEuler = this.gameObject.transform.localEulerAngles;
        tempEuler.z = -5f;
        this.gameObject.transform.localEulerAngles = tempEuler;

        //unparent
        if (this.gameObject.transform.parent != null)
        {
            //unparent
            this.gameObject.transform.parent = null;
        }

        plankState = PlankState.beingplaced;
        

    }

    public void PickUpPlankCall(GameObject playerRef)
    {
        print(this.gameObject + " Picked Up");

        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //turn off collider
        this.gameObject.GetComponent<Collider>().enabled = false;

        //print("rigid off and kinematic");

        //set to not hit players
        SetToNotHitPlayers(); //turn on then off when board slamming

        print(plankAnim != null);

        //animation
        StartCoroutine(plankAnim.PickUpPlankAnim(playerRef));
    }

    public void PickUpPlank(GameObject playerRef)
    { 
        //pick it up (done by player)

        //tell plank what player to look at
        this.gameObject.GetComponent<SnapTest2>().player = playerRef;

        //get player who placed me
        playerWhoPlacedMe = playerRef.GetComponent<PlayerController>();

        

       //StartCoroutine(plankAnim.PickUpPlankAnim());

        /*
        //destroy rigidbody
        if (this.gameObject.GetComponent<Rigidbody>() != null)
        {
            Destroy(this.gameObject.GetComponent<Rigidbody>());
        }
        */

        //held
        plankState = PlankState.held;
    }

    public void DropPlank()
    {
        print(this.gameObject + " Dropped");
        //turn on collider

        //to stop it from falling through floor
        this.gameObject.transform.position += new Vector3(0, .2f, 0);

        this.gameObject.GetComponent<Collider>().enabled = true;

        //not trigger
        this.gameObject.GetComponent<Collider>().isTrigger = false;

        //make collider full sized
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
        this.gameObject.GetComponent<BoxCollider>().center = Vector3.zero;


        //turn off artificial gravity
        snapRef.GravitySwitch(false);

        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;

        SetToHitPlayers();

        //StartCoroutine(DropDelaySetToHitPlayers());

        /*
        //if no rigidbody add one
        if (this.gameObject.GetComponent<Rigidbody>() == null)
        {
            this.gameObject.AddComponent<Rigidbody>();
        }
        */

        plankState = PlankState.dropped;
    }

    public void PlacePlank()
    {
        //print(this.gameObject + "Placed");

        /*
        //get rid of rigidbody
        if (this.gameObject.GetComponent<Rigidbody>() != null)
        {
            Destroy(this.gameObject.GetComponent<Rigidbody>());
        }
        */

        //CHECK TO MAKE SURE NOT BEING PLACED ON GROUND (needs fixing)
        /*

        RaycastHit hitInfo;

        //raycast down to make sure not on ground
        if (Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hitInfo, .1f))
        {
            //print("hit " + hitInfo.collider.gameObject.name);

            //make sure not too soon after plank initially created

            print("Time plank" + (Time.time - timePlankCreated));

            if (hitInfo.collider.gameObject.GetComponent<PlankManager>() == null && hitInfo.collider.gameObject.GetComponent<PlayerController>() == null 
                && (Time.time - timePlankCreated) < 1f)
            {
                //if it is drop it and return
                DropPlank();
                //gets 0 points
                //GameManager.S.player1.GetComponent<FlashyPoints>().ShowPointsGained(transform.position, 0);
                return;
            }
        }
        */

        if(this.gameObject.GetComponent<Rigidbody>() != null)
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        

        this.gameObject.GetComponent<Collider>().isTrigger = false;


        //turn off artifical gravity (stop from moving)
        snapRef.GravitySwitch(false);

        //this fixes boards snapping to wrong places
        //if z mildly rotated make it 0
        if (this.gameObject.transform.eulerAngles.z != 0)
        {
            Vector3 temp;
            temp = this.gameObject.transform.eulerAngles;
            temp.z = 0;
            this.gameObject.transform.eulerAngles = temp;
        }

        //turn on collider
        this.gameObject.GetComponent<Collider>().enabled = true;
        //make trigger
        this.gameObject.GetComponent<Collider>().isTrigger = false;

        //make collider full sized
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1,1,1);
        this.gameObject.GetComponent<BoxCollider>().center = Vector3.zero;

        //give them points
        if (playerWhoPlacedMe != null)
        {
            playerWhoPlacedMe.GetComponent<Points>().AddPointsForBoardPlace();
            //show
            GameManager.S.player1.GetComponent<FlashyPoints>().ShowPointsGained(transform.position, GameManager.S.player1.GetComponent<Points>().pointsForBoardPlace);
            /*playerWhoPlacedMe.GetComponent<FlashyPoints>().ShowPointsGained(playerWhoPlacedMe.transform.position,
                playerWhoPlacedMe.GetComponent<Points>().pointsForBoardPlace);*/
        }

        plankState = PlankState.placed;
    }


    public void SetToNotHitPlayers()
    {
        if(GameManager.S != null && GameManager.S.player1 != null & GameManager.S.player2 != null)
        {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player1.GetComponent<Collider>(), true);
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player2.GetComponent<Collider>(), true);
            if(GameManager.S.player3 != null)
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player3.GetComponent<Collider>(), true);
            }
            if (GameManager.S.player4 != null)
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player4.GetComponent<Collider>(), true);
            }
           

            //also if in windy level don't let hit wind
            if (GameObject.FindObjectOfType<WindMechanic>() != null)
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameObject.FindObjectOfType<WindMechanic>().GetComponent<Collider>(), true);
            }

            
            //also ignore anything with ignore plank when being placed
            if (GameObject.FindObjectOfType<IgnoreMeWhenPlacingPlank>() != null)
            {
                //get list of all
                IgnoreMeWhenPlacingPlank[] ignoreStuff = FindObjectsOfType<IgnoreMeWhenPlacingPlank>();

                //ignore them all
                for (int index = 0; index < ignoreStuff.Length; index++)
                {
                    Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), ignoreStuff[index].gameObject.GetComponent<Collider>(), true);
                }
            }
            
        }
        else
        {
            print("Error: No GameManager with references to players");
        }
      
    }
    public void SetToHitPlayers()
    {
        if (GameManager.S != null && GameManager.S.player1 != null & GameManager.S.player2 != null)
        {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player1.GetComponent<Collider>(), false);
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player2.GetComponent<Collider>(), false);
            if (GameManager.S.player3 != null)
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player3.GetComponent<Collider>(), false);
            }
            if (GameManager.S.player4 != null)
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player4.GetComponent<Collider>(), false);
            }
            //also if in windy level let hit wind
            if (GameObject.FindObjectOfType<WindMechanic>() != null)
            {
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameObject.FindObjectOfType<WindMechanic>().GetComponent<Collider>(), false);
            }

            //also ignore anything with ignore plank when being placed
            if (GameObject.FindObjectOfType<IgnoreMeWhenPlacingPlank>() != null)
            {
                //get list of all
                IgnoreMeWhenPlacingPlank[] ignoreStuff = FindObjectsOfType<IgnoreMeWhenPlacingPlank>();

                //ignore them all
                for (int index = 0; index < ignoreStuff.Length; index++)
                {
                    Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), ignoreStuff[index].gameObject.GetComponent<Collider>(), false);
                }
            }

        }
        else
        {
            print("Error: No GameManager with references to players");
        }
    }


}
