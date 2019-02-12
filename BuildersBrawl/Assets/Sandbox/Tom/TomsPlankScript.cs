using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomsPlankScript : MonoBehaviour
{

    //NOT DONE
    //I SHOULDN"T BE DOING THIS


        //defulat is droppng
    public enum PlankState
    {
        dropped,
        beingplaced,
        held,
        placed
    }

    public PlankState plankState;
    [HideInInspector]
    public GameObject[] nodes;
    //[HideInInspector]
    //public Quaternion plankRotation;

    GameObject myParentPlayer;

    private void Start()
    {
        PopulateNodeArray();
        //plankRotation = transform.rotation;

        //when being made plank is either being held by player, is already placed or is just a plank that 


        //defualt is dropping
        if(plankState == PlankState.beingplaced)
        {
            PlacingPlank();
        }
        else if (plankState == PlankState.held)
        {
            
        }
        else if (plankState == PlankState.dropped)
        {

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
        if(myParentPlayer != null)
        {
            //unparent
            

            //set parent to null
            myParentPlayer = null;
        }

        plankState = PlankState.beingplaced;
    }

    public void PickUpPlank(GameObject parent)
    {
        //destoryy rigidbody?

        //turn off collider
        this.gameObject.GetComponent<Collider>().enabled = true;

        //make child of parent


        //held
        plankState = PlankState.held;
    }

    public void DropPlank()
    {
        //turn on collider
        this.gameObject.GetComponent<Collider>().enabled = true;

        //make hitable by players

        //unparent
        if (myParentPlayer != null)
        {
            //unparent


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

    private void OnTriggerEnter(Collider other)
    {
        GameObject snappableNode;
        GameObject closestNode;

        if (other.gameObject.GetComponent<TomsPlankScript>() != null && other.gameObject.GetComponent<TomsPlankScript>().plankState.Equals(PlankState.placed))
        {
                //destory this planks rigidbdy
                Destroy(this.gameObject.GetComponent<Rigidbody>());
                
                //snap it   
                snappableNode = other.gameObject.GetComponent<TomsPlankScript>().nodes[0];
                closestNode = FindClosestNode(snappableNode);
                SnapNodes(snappableNode, closestNode);
                
                //make this plank placed
                other.gameObject.GetComponent<TomsPlankScript>().plankState = PlankState.placed;

        }
        else
        {
            //make it drop

        }
    }

    private void PopulateNodeArray()
    {
        List<GameObject> nodeList = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.tag.Equals("Node"))
                nodeList.Add(transform.GetChild(i).gameObject);
        }

        nodes = new GameObject[nodeList.Count];

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = nodeList[i];
        }
    }

    private void SnapNodes(GameObject snapNode, GameObject closeNode)
    {
        /*
         * Snap to the Node's X and Z
         * Snap to Plank's Y + .001
         */
        Vector3 snapPoint = new Vector3(closeNode.transform.position.x,
            closeNode.transform.parent.transform.position.y, closeNode.transform.position.z);

        snapNode.transform.parent.transform.position = snapPoint;

        Vector3 offSet = snapNode.transform.position - closeNode.transform.position;

        snapNode.transform.parent.transform.position =
            new Vector3(snapNode.transform.parent.transform.position.x - offSet.x,
            snapNode.transform.parent.transform.position.y + .0034f,
            snapNode.transform.parent.transform.position.z - offSet.z);

        /*snapNode.transform.parent.rotation = new Quaternion(0, snapNode.transform.parent.rotation.y, 0,
            snapNode.transform.parent.rotation.w);*/
        //snapNode.transform.parent.rotation = snapNode.transform.parent.GetComponent<SnapTest2>().plankRotation;
    }

    private GameObject FindClosestNode(GameObject snappableNode)
    {
        GameObject closestNode = nodes[0];
        float smallestDistance = 200f;

        for (int i = 0; i < nodes.Length; i++)
        {
            float distance = Vector3.Distance(snappableNode.transform.position,
                nodes[i].transform.position);

            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestNode = nodes[i];
            }
        }

        return closestNode;
    }
}
