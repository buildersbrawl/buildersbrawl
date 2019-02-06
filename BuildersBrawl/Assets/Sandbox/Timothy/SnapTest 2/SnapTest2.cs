using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTest2 : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] nodes;

    private void Start()
    {
        nodes = new GameObject[transform.childCount];

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = transform.GetChild(i).gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject snappableNode;
        GameObject closestNode;

        //other = falling Plank
        if (other.tag.Equals("Plank") && other.gameObject.GetComponent<Rigidbody>() != null)
        {
            Debug.Log("here");
            Destroy(other.gameObject.GetComponent<Rigidbody>());

            snappableNode = FindClosestEndNode(other.gameObject);
            closestNode = FindClosestNode(snappableNode, other.gameObject);
            SnapNodes(snappableNode, closestNode);

        }
    }

    private void SnapNodes(GameObject snappableNode, GameObject snapTo)
    {
        Debug.Log("Here");
    }

    private GameObject FindClosestEndNode(GameObject fallingPlank)
    {
        SnapTest2 endSnap = fallingPlank.GetComponent<SnapTest2>();
        if (Vector3.Distance(endSnap.nodes[0].transform.position, transform.position)
            < Vector3.Distance(endSnap.nodes[nodes.Length - 1].transform.position, transform.position))
        {
            return endSnap.nodes[0];
        }
        else
            return endSnap.nodes[nodes.Length - 1];
    }

    private GameObject FindClosestNode(GameObject snappableNode, GameObject snaptoPlank)
    {
        SnapTest2 checkSnap = snaptoPlank.GetComponent<SnapTest2>();
        float smallestDistance = 0;
        GameObject node = null;

        for (int i = 0; i < nodes.Length; i++)
        {
            float distanceCheck = Vector3.Distance(snappableNode.transform.position, 
                checkSnap.nodes[i].transform.position);

            if (distanceCheck < smallestDistance)
            {
                smallestDistance = distanceCheck;
                node = checkSnap.nodes[i];
            }
        }

        return node;
    }
}
