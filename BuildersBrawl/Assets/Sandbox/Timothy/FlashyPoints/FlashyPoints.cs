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
        pointsText.transform.Rotate(new Vector3(0, -90));
        pointsText.GetComponent<TextMesh>().text = "+" + points.ToString();
        StartCoroutine(DestroyText(pointsText));
    }

    private IEnumerator DestroyText(GameObject text)
    {
        yield return new WaitForSeconds(textActiveTime);
        Destroy(text);
    }
}
