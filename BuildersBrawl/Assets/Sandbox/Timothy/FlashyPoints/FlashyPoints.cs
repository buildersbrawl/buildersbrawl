using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashyPoints : MonoBehaviour
{
    public GameObject pointsTextPrefab;
    public float textActiveTime = 3;

    public void ShowPointsGained(Vector3 instantiatePos, int points)
    {
        GameObject pointsText = Instantiate(pointsTextPrefab, instantiatePos, Quaternion.identity);
        if(GameManager.S.cameraRef.GetComponent<CameraController>().cameraOptions == CameraController.CameraOptions.side)
        {
            pointsText.transform.Rotate(new Vector3(0, -90));
        }
        else if(GameManager.S.cameraRef.GetComponent<CameraController>().cameraOptions == CameraController.CameraOptions.front)
        {
            pointsText.transform.Rotate(new Vector3(0, 180));
        }
        pointsText.GetComponent<TextMesh>().text = "+" + points.ToString();
        StartCoroutine(DestroyText(pointsText));
    }

    private IEnumerator DestroyText(GameObject text)
    {
        yield return new WaitForSeconds(textActiveTime);
        Destroy(text);
    }
}
