using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankManager : MonoBehaviour
{
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
        //make trigger
        this.gameObject.GetComponent<Collider>().isTrigger = false;

        //default is dropping
        if (plankState == PlankState.beingplaced)
        {
            PlacingPlank();
        }
        else if (plankState == PlankState.held)
        {
            PickUpPlank();
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
        //don't let plank hit players
        SetToNotHitPlayers();

        //turn on artificial gravity
        //snapRef.TurnOnGravity

        //unparent
        if (this.gameObject.transform.parent != null)
        {
            //unparent
            this.gameObject.transform.parent = null;
        }

        plankState = PlankState.beingplaced;
    }

    public void PickUpPlank()
    {
        //pick it up (done by player)

        //set to not hit players
        SetToNotHitPlayers(); //turn on then off when board slamming

        //destroy rigidbody
        if (this.gameObject.GetComponent<Rigidbody>() != null)
        {
            Destroy(this.gameObject.GetComponent<Rigidbody>());
        }

        //turn off collider
        this.gameObject.GetComponent<Collider>().enabled = false;


        //held
        plankState = PlankState.held;
    }

    public void DropPlank()
    {
        //turn on collider
        this.gameObject.GetComponent<Collider>().enabled = true;

        //turn off artificial gravity
        //snapRef.TurnOffGravity

        //make hitable by players
        SetToHitPlayers();

        //if no rigidbody add one
        if (this.gameObject.GetComponent<Rigidbody>() == null)
        {
            this.gameObject.AddComponent<Rigidbody>();
        }

        plankState = PlankState.dropped;
    }

    public void PlacePlank()
    {
        //get rid of rigidbody
        if (this.gameObject.GetComponent<Rigidbody>() != null)
        {
            Destroy(this.gameObject.GetComponent<Rigidbody>());
        }

        //turn off artifical gravity (stop from moving)
        //snapRef.TurnOffGravity

        //make trigger
        this.gameObject.GetComponent<Collider>().isTrigger = true;

        plankState = PlankState.placed;
    }


    public void SetToNotHitPlayers()
    {
        if(GameManager.S != null && GameManager.S.player1 != null & GameManager.S.player2 != null)
        {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), GameManager.S.player1.GetComponent<Collider>(), true);
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
        }
        else
        {
            print("Error: No GameManager with references to players");
        }
    }


}
