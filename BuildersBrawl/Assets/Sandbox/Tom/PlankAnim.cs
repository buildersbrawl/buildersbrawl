using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankAnim : MonoBehaviour
{

    PlankManager plankManager;

    public void SetPlankManager(PlankManager pM)
    {
        plankManager = pM;
    }


    public IEnumerator PutDownPlankAnim()
    {
        print("parent down" + this.transform.parent);

        float interpolationPercent = 0;
        float interpolationIncrement = .05f;

        Vector3 startPosition = this.gameObject.transform.localPosition;
        Vector3 startRotation = this.gameObject.transform.localEulerAngles;

        Vector3 endPosition = startPosition + new Vector3(0, -1.4f, 2.4f);
        Vector3 endRotation = this.gameObject.transform.localEulerAngles + new Vector3(0, 0, 160);

        bool boardAnimCont = true;

        while (boardAnimCont)
        {
            yield return new WaitForSeconds(.01f);


            interpolationPercent += interpolationIncrement;

            this.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, interpolationPercent);
            this.gameObject.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, interpolationPercent);

            //determines switch and end
            if (interpolationPercent >= 1f)
            {
                boardAnimCont = false;
            }
        }

        plankManager.PlacingPlank();

    }

    public IEnumerator PickUpPlankAnim(GameObject playerRef)
    {
        print("parent up" + this.transform.parent);

        float interpolationPercent = 0;
        float interpolationIncrement = .05f;

        Vector3 startPosition = this.gameObject.transform.localPosition;
        Vector3 startRotation = this.gameObject.transform.localEulerAngles;

        Vector3 endPosition = startPosition + new Vector3(0, 1.4f, -2.4f);
        Vector3 endRotation = this.gameObject.transform.localEulerAngles + new Vector3(0, 0, -160);

        bool boardAnimCont = true;

        while (boardAnimCont)
        {
            yield return new WaitForSeconds(.01f);


            interpolationPercent += interpolationIncrement;

            this.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, interpolationPercent);
            this.gameObject.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, interpolationPercent);

            //determines switch and end
            if (interpolationPercent >= 1f)
            {
                boardAnimCont = false;
            }
        }

        plankManager.PickUpPlank(playerRef);
    }
}
