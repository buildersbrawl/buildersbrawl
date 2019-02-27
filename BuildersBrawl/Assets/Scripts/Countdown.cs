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
    // Start is called before the first frame update
    void Start()
    {
        delayStart = delay;
        cd.text = count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        delay -= Time.deltaTime;
        if(delay < 0)
        {
            if (count > 1)
            {
                count--;
                cd.text = count.ToString();
            }
            else
            {
                cd.color = Color.green;
                cd.text = "BEGIN!";
                count--;
            }

            if (count < 0)
            {
                cd.enabled = false;
                cd_panel.SetActive(false);
            }
            delay = delayStart;
        }
    }
}
