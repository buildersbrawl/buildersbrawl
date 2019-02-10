using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTest2 : MonoBehaviour
{
    public enum PlankState
    {
        beingplaced,
        held,
        dropped,
        placed
    }

    public PlankState plankState;
    [HideInInspector]
    public GameObject[] nodes;
    //[HideInInspector]
    //public Quaternion plankRotation;

    private void Start()
    {
        PopulateNodeArray();
        //plankRotation = transform.rotation;
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject snappableNode;
        GameObject closestNode;

        if (other.tag.Equals("Plank") && other.gameObject.GetComponent<Rigidbody>() != null
            && other.gameObject.GetComponent<SnapTest2>().plankState.Equals(PlankState.beingplaced))
        {
            Destroy(other.gameObject.GetComponent<Rigidbody>());

            snappableNode = other.gameObject.GetComponent<SnapTest2>().nodes[0];
            closestNode = FindClosestNode(snappableNode);
            SnapNodes(snappableNode, closestNode);

            other.gameObject.GetComponent<SnapTest2>().plankState = PlankState.placed;
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
