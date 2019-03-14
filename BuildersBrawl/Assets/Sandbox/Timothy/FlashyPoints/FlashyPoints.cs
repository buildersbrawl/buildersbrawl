using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashyPoints : MonoBehaviour
{
    public GameObject pointsTextPrefab;
    public float textActiveTime = 3;
    public int uptravelDistance = 5;
    

    public void ShowPointsGained(Vector3 instantiatePos, int points)
    {
        GameObject pointsText = Instantiate(pointsTextPrefab, instantiatePos, Quaternion.identity);
        pointsText.transform.LookAt(GameManager.S.cameraRef.transform.position);
        //pointsText.transform.rotation.SetFromToRotation(new Vector3(0, 180, 0), transform.up);
        //pointsText.transform.Rotate(new Vector3(0, 180));
        /*
        if(GameManager.S.cameraRef.GetComponent<CameraController>().cameraOptions == CameraController.CameraOptions.side)
        {
            pointsText.transform.Rotate(new Vector3(0, -90));
        }
        else if(GameManager.S.cameraRef.GetComponent<CameraController>().cameraOptions == CameraController.CameraOptions.front)
        {
            pointsText.transform.Rotate(new Vector3(0, 180));
        }*/
        pointsText.GetComponent<TextMesh>().text = "+" + points.ToString();
        //StartCoroutine(FadeText(pointsText));
    }

    /*
    private IEnumerator FadeText(GameObject text)
    {

        yield return new WaitForSeconds(textActiveTime);
        Destroy(text);
    }*/
}
