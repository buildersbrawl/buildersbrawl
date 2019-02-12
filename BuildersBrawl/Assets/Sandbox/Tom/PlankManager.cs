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


    GameObject myParentPlayer;

    private void Start()
    {
        
        //defualt is dropping
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

        //get rid of rigidbody?? what about gravity?

        //unparent
        if (myParentPlayer != null)
        {
            //unparent


            //set parent to null
            myParentPlayer = null;
        }

        plankState = PlankState.beingplaced;
    }

    public void PickUpPlank()
    {
        //pick it up
        //boxHitInfo.collider.gameObject.GetComponent<SnapTest2>().PickUpPlank(this.gameObject)
        
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

        //make hitable by players

        //unparent
        if (this.transform.parent != null)
        {
            //unparent
            this.transform.parent = null;

            //set parent to null
            myParentPlayer = null;
        }

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


        plankState = PlankState.placed;
    }



}
