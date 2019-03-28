using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public int allowedTime; //This is in seconds 180 seconds or 3 min for now
    public Text timerText;
    public GameObject drawImage;

    private void Update()
    {
        float currentTime = allowedTime - Time.time;

        if (currentTime > 0)
        {
            string minutes = Mathf.Floor(currentTime / 60).ToString("00");
            string seconds = Mathf.Floor(currentTime % 60).ToString("00");

            timerText.text = minutes + ":" + seconds;
        }
        else if (currentTime <= 0)
        {
            //End game in Draw
            drawImage.SetActive(true);
            //Wipe Points
            Debug.Log("Time Limit Reached");
        }
    }

}
