using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTest2 : MonoBehaviour
{

    public GameObject player;
    [HideInInspector]
    public GameObject[] nodes;
    public bool gravity = false;
    public float fallSpeed = 3f;

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        PopulateNodeArray();
    }


    private void FixedUpdate()
    {
        if (gravity)
        {
            /*Collider plankCollider = this.gameObject.GetComponent<Collider>();
            if (!Physics.BoxCast(plankCollider.bounds.center, plankCollider.bounds.size / 2, Vector3.down, transform.rotation, .01f))
            {
                
                transform.position += Vector3.down * Time.fixedDeltaTime;
            }*/
            transform.position += Vector3.down * fallSpeed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject snappableNode;
        GameObject closestNode;

        if (other.gameObject.GetComponent<PlayerController>() != null || (other.gameObject.transform.parent != null && other.gameObject.transform.parent.GetComponent<PlayerController>() != null) || this.gameObject.GetComponent<PlankManager>().plankState == PlankManager.PlankState.placed) //make sure to ignore if this plank is already placed
        {
            //ignore players, player children and if this is a placed plank
            print("ignoring");
        }
        else if (other.tag.Equals("Plank") && other.gameObject.GetComponent<PlankManager>().plankState.Equals(PlankManager.PlankState.placed))
        {
            print("I am " + this.gameObject.name);
            print("triggered by " + other.gameObject.name);
            //other.GetComponent<SnapTest2>().player = player;
            GravitySwitch(false);
            snappableNode = FindClosestEndNode(gameObject);
            closestNode = FindClosestSnapLoc(snappableNode, other.gameObject);
            SnapNodes(snappableNode, closestNode);

            gameObject.GetComponent<PlankManager>().PlacePlank();
        }
        else
        {
            print("dropped becasuse hit not placed plank: " + other.gameObject.name);
            gameObject.GetComponent<PlankManager>().DropPlank();
        }
    }


    private void PopulateNodeArray()
    {
        List<GameObject> nodeList = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.tag.Equals("Node"))
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
    }

    private GameObject FindClosestEndNode(GameObject fallingPlank)
    {
        SnapTest2 nodeRef = fallingPlank.GetComponent<SnapTest2>();

        if (Vector3.Distance(nodeRef.nodes[0].transform.position, player.transform.position)
            <= Vector3.Distance(nodeRef.nodes[nodes.Length - 1].transform.position, player.transform.position))
        {
            return fallingPlank.GetComponent<SnapTest2>().nodes[0];
        }
        else
        {
            return fallingPlank.GetComponent<SnapTest2>().nodes[nodes.Length - 1];
        }
    }

    private GameObject FindClosestSnapLoc(GameObject snappableNode, GameObject placedPlank)
    {
        GameObject closestNode = placedPlank.GetComponent<SnapTest2>().nodes[0];
        float smallestDistance = 200f;
        
        for (int i = 0; i < placedPlank.GetComponent<SnapTest2>().nodes.Length; i++)
        {
            float distance = Vector3.Distance(snappableNode.transform.position,
                placedPlank.GetComponent<SnapTest2>().nodes[i].transform.position);

            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestNode = placedPlank.GetComponent<SnapTest2>().nodes[i];
            }
        }

        return closestNode;
    }

    public void GravitySwitch(bool switchState)
    {
        gravity = switchState;
    }
}
