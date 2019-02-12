using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCastStuff : MonoBehaviour
{
    [SerializeField]
    private Vector3 boxCastOffset = new Vector3(0, 0, 0);
    [SerializeField]
    private Vector3 boxCasthalfSize = new Vector3(.5f, .5f, .1f);
    [SerializeField]
    private float boxCastMaxDistance = 1;
    private Quaternion playerRotation;
    private Vector3 playerForward;

    RaycastHit[] boxHitInfo;

    public bool holdingBoard = false;

    [Header("TestCubes")]
    public bool testCubes;
    public GameObject startCube;
    public GameObject endCube;

    private void Start()
    {
        if(startCube = null)
        {
            startCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        if (endCube = null)
        {
            endCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }

    public void TestBoxCast(GameObject startCube, GameObject endCube)
    {
        startCube.GetComponent<Collider>().enabled = false;
        startCube.transform.position = this.gameObject.transform.position + boxCastOffset;
        startCube.transform.rotation = playerRotation;
        startCube.transform.localScale = boxCasthalfSize * 2;

        endCube.GetComponent<Collider>().enabled = false;
        endCube.transform.position = this.gameObject.transform.position + boxCastOffset + (playerForward * boxCastMaxDistance);
        endCube.transform.rotation = playerRotation;
        endCube.transform.localScale = boxCasthalfSize * 2;
    }


}
