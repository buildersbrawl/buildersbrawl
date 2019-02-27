using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        placed
    }

    public PlankState plankState;

    private SnapTest2 snapRef;

    private PlayerController playerWhoPlacedMe;

    private void Start()
    {
        Init();
    }

    private void Init()
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

        //not trigger
        this.gameObject.GetComponent<Collider>().isTrigger = false;

        //default is dropping
        if (plankState == PlankState.beingplaced)
        {
            PlacingPlank();
        }
        else if (plankState == PlankState.held)
        {
            //PickUpPlank();
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


    public void PlacingPlank()
    {
        print(this.gameObject + " Placing");

        //don't let plank hit players
        SetToNotHitPlayers();

        //turn on artificial gravity
        snapRef.GravitySwitch(true);

        //turn on collider
        this.gameObject.GetComponent<Collider>().enabled = true;

        //make trigger
        this.gameObject.GetComponent<Collider>().isTrigger = true;

        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //unparent
        if (this.gameObject.transform.parent != null)
        {
            //unparent
            this.gameObject.transform.parent = null;
        }

        plankState = PlankState.beingplaced;
    }

    public void PickUpPlank(GameObject playerRef)
    {
        print(this.gameObject + " Picked Up");
        //pick it up (done by player)

        //tell plank what player to look at
        this.gameObject.GetComponent<SnapTest2>().player = playerRef;

        //get player who placed me
        playerWhoPlacedMe = playerRef.GetComponent<PlayerController>();

        //set to not hit players
        SetToNotHitPlayers(); //turn on then off when board slamming

        /*
        //destroy rigidbody
        if (this.gameObject.GetComponent<Rigidbody>() != null)
        {
            Destroy(this.gameObject.GetComponent<Rigidbody>());
        }
        */
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        //turn off collider
        this.gameObject.GetComponent<Collider>().enabled = false;


        //held
        plankState = PlankState.held;
    }

    public void DropPlank()
    {
        print(this.gameObject + " Dropped");
        //turn on collider
        this.gameObject.GetComponent<Collider>().enabled = true;

        //not trigger
        this.gameObject.GetComponent<Collider>().isTrigger = false;

        //turn off artificial gravity
        snapRef.GravitySwitch(false);

        //make hitable by players
        SetToHitPlayers();

        /*
        //if no rigidbody add one
        if (this.gameObject.GetComponent<Rigidbody>() == null)
        {
            this.gameObject.AddComponent<Rigidbody>();
        }
        */
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;

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

        

        //give them points
        playerWhoPlacedMe.GetComponent<Points>().AddPointsForBoardPlace();


        plankState = PlankState.placed;
    }


    public void SetToNotHitPlayers()
    {
        if(GameManager.S != null && GameManager.S.player1 != null & GameManager.S.player2 != null)
        {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player1.GetComponent<Collider>(), true);
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player2.GetComponent<Collider>(), true);
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
        }
        else
        {
            print("Error: No GameManager with references to players");
        }
    }


}
