using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour
{
    public GameObject snapToThis;
    public GameObject snappableObject;

    private Renderer objectRenderer;
    private Renderer snappableObjectRenderer;

    private void Start()
    {
        objectRenderer = snapToThis.GetComponent<Renderer>();
        snappableObjectRenderer = snappableObject.GetComponent<Renderer>();
    }

    public void Snap()
    {
        /*Vector3 objectSize = objectRenderer.bounds.size;
        Vector3 objectCenter = objectRenderer.bounds.center;

        Vector3 rightface_top = new Vector3(objectCenter.x + objectRenderer.bounds.extents.x, objectCenter.y + objectRenderer.bounds.extents.y);

        Vector3 snappablePoint = new Vector3(rightface_top.x + snappableObjectRenderer.bounds.extents.x,
            rightface_top.y - snappableObjectRenderer.bounds.extents.y);

        Instantiate(snappableObject, snappablePoint, Quaternion.identity);*/

        Vector3 snappablePoint = FindSnapPoint(snapToThis);

        Instantiate(snappableObject, snappablePoint, Quaternion.identity);
    }

    private Vector3 FindSnapPoint(GameObject testObject)
    {
        Renderer testRenderer = testObject.GetComponent<Renderer>();
        Vector3 topcenterPoint = new Vector3(testRenderer.bounds.center.x, testRenderer.bounds.center.y + testRenderer.bounds.extents.y);
        Vector3 snapPoint = new Vector3(topcenterPoint.x, topcenterPoint.y - snappableObjectRenderer.bounds.extents.y, 
            topcenterPoint.z - testRenderer.bounds.extents.z - snappableObjectRenderer.bounds.extents.z);

        return snapPoint;
    }
}
