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

        if (other.tag.Equals("Plank") && other.gameObject.GetComponent<Rigidbody>() != null)
        {
            Destroy(other.gameObject.GetComponent<Rigidbody>());

            snappableNode = other.gameObject.GetComponent<SnapTest2>().nodes[0];
            //Debug.Log("Snappable Node: " + snappableNode.transform.localPosition);
            closestNode = FindClosestNode(snappableNode);

        }
    }

    private GameObject FindClosestNode(GameObject snappableNode)
    {
        GameObject closestNode = nodes[0];
        float smallestDistance = 200f;

        //float distance = Vector3.Distance(snappableNode.transform.position, nodes[2].transform.position);
        //Debug.Log("Normal Position Distance: " + distance);
        //float distance = Vector3.Distance(snappableNode.transform.localPosition, nodes[2].transform.localPosition);
        //Debug.Log("Local Position Distance: " + distance);
        
        for (int i = 0; i < nodes.Length; i++)
        {
            float distance = Vector3.Distance(snappableNode.transform.position,
                nodes[i].transform.position);
            Debug.Log("Node " + i + ":" + distance);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                //Debug.Log(smallestDistance);
                closestNode = nodes[i];
            }
        }

        return closestNode;
    }
}
