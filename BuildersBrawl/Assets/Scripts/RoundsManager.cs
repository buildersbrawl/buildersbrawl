using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundsManager : MonoBehaviour
{
    public static RoundsManager R;

    public int round;
    public int maxRounds;

    void Awake()
    {
        //if another one of these get rid of this one
        if (RoundsManager.R == null)
        {
            R = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        maxRounds = 3;
    }

    public void Reset()
    {
        round = 1;
    }
    
    /*//TESTING PURPOSES ONLY
    public GameObject testButton;

    public void AddRound()
    {
        round++;
    }

    void Update()
    {
        CheckRounds();
    }

    void CheckRounds()
    {
        if(round >= maxRounds)
        {
            testButton.SetActive(false);
        }
    }*/
}
