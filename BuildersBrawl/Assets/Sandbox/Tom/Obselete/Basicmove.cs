using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basicmove : MonoBehaviour
{
    public GameObject moveObj1;
    public GameObject moveObj2;

    float inside = 1;
    float outside = 5;

    float desiredLocation = 1;
    float startLocation;
    int loopFactor = 0;
    [SerializeField]
    float time = 0;

    [SerializeField]
    Vector3 actualLoc;

    [SerializeField]
    bool stopMovement;

    public void Start()
    {
        startLocation = inside;
    }

    public void Update()
    {
        if (stopMovement)
        {
            return;
        }
        //.3 so more like 3 seconds
        time += (Time.deltaTime * .5f);

        switch (loopFactor)
        {
            case 0:
                //go out
                desiredLocation = outside;
                startLocation = inside;
                break;
            case 1:
                //stayy out
                desiredLocation = outside;
                startLocation = outside;
                break;
            case 2:
                //go in
                desiredLocation = inside;
                startLocation = outside;
                break;
            case 3:
                //stay in
                desiredLocation = inside;
                startLocation = inside;
                break;

            default:
                break;
        }

        actualLoc = moveObj1.transform.position;
        actualLoc.x = Mathf.Lerp(startLocation, desiredLocation, time);
        moveObj1.transform.position = actualLoc;

        actualLoc = moveObj2.transform.position;
        actualLoc.x = Mathf.Lerp(-startLocation * .5f, -desiredLocation * .5f, time);
        moveObj2.transform.position = actualLoc;


        if (time >= 1)
        {
            //rotate
            loopFactor++;
            if(loopFactor > 3)
            {
                loopFactor = 0;
            }
            //time = 0
            time = 0;
        }
    }
}
