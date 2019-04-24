using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{

    private int count = 5;

    public float delay;
    private float delayStart;

    public Text cd;
    public GameObject cd_panel;
    public Image cd_icon;

    public bool startTimer = false;
    [HideInInspector]
    public bool countDown = true;

    // Start is called before the first frame update
    void Start()
    {
        
        delay = 1;

        delayStart = delay;
        cd.text = count.ToString();

        //disables player controller
        GameManager.S.player1.GetComponent<PlayerController>().enabled = false;
        GameManager.S.player2.GetComponent<PlayerController>().enabled = false;
        GameManager.S.player3.GetComponent<PlayerController>().enabled = false;
        GameManager.S.player4.GetComponent<PlayerController>().enabled = false;

        cd_icon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("startTime = " + startTimer);
        if (startTimer)
        {
            delay -= Time.deltaTime;
            if (delay < 0)
            {
                if (count > 1)
                {
                    count--;
                    cd.text = count.ToString();
                }
                else
                {
                    cd.enabled = false;
                    cd_icon.enabled = true;
                    count--;
                }

                if (count < 0)
                {
                    cd.enabled = false;
                    cd_panel.SetActive(false);
                    cd_icon.enabled = false;

                    //re-enables player controller
                    GameManager.S.player1.GetComponent<PlayerController>().enabled = true;
                    GameManager.S.player2.GetComponent<PlayerController>().enabled = true;
                    GameManager.S.player3.GetComponent<PlayerController>().enabled = true;
                    GameManager.S.player4.GetComponent<PlayerController>().enabled = true;
                    countDown = false;
                }
                delay = delayStart;
            }

            /*delay -= Time.deltaTime;
        if(delay < 0)
        {
            if (count > 1)
            {
                count--;
                cd.text = count.ToString();
            }
            else
            {
                cd.enabled = false;
                cd_icon.enabled = true;
                count--;
            }

            if (count < 0)
            {
                cd.enabled = false;
                cd_panel.SetActive(false);
                cd_icon.enabled = false;

                //re-enables player controller
                GameManager.S.player1.GetComponent<PlayerController>().enabled = true;
                GameManager.S.player2.GetComponent<PlayerController>().enabled = true;
                GameManager.S.player3.GetComponent<PlayerController>().enabled = true;
                GameManager.S.player4.GetComponent<PlayerController>().enabled = true;
            }
            delay = delayStart;*/
        }
    }
}
