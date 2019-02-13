using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Graphic m_Graphic;
    public Color[] levels;
    public int levelNumber;
    public int show = 0;
    public bool chosenShown = false;

    public string[] levelNames;
    public string chosenLevel;

    //used to adjust time between showing each image
    public float waitTime = 0.1f;
    private float timer = 0.0f;
    public float maxWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        //Control what is displayed on the image
        m_Graphic = GetComponent<Graphic>();
        Debug.Log(levels.Length);

        //Immediately chooses the next level
        ChooseLevel();
    }

    // Update is called once per frame
    void Update()
    {
        //If the random level is not shown, then system will cycle through levels that will be displayed
        if (!chosenShown)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
             CycleChoices();
                timer -= waitTime;
                if(waitTime < maxWaitTime)
                {
                    waitTime += 0.1f;
                }
            }
        }
    }

    void FixedUpdate()
    {
        //If the random level is not shown, then the levels will be displayed in rapid succession
        if (!chosenShown)
        {
            RotateLevels();
        }
        //Displays the randomly chosen level
        if (waitTime >= maxWaitTime && show == levelNumber)
        {
            DisplayChosen();
        }
    }

    void ChooseLevel()
    {//Takes a random number from the levels array
        levelNumber = Random.Range(0, levels.Length);
        //Stores level name
        chosenLevel = levelNames[levelNumber];
    }
    void CycleChoices()
    {
        //Cycles through levels in the array to be shown. Resets to show top level (0) once the display hits the bottom of the list
        if(show >= levels.Length-1)
        {
            show = 0;
        }
        else
        {
            show += 1;
        }
    }

    void RotateLevels()
    {
        //displays level based on the order in the array
        m_Graphic.color = levels[show];
    }

    void DisplayChosen()
    {
        //Displays the chosen level after a certain amount of time. Can be changed to display level after all players are ready.
        m_Graphic.color = levels[levelNumber];

        //Stops random level stuff in order to not slow down Unity
        chosenShown = true;
    }

    public void StartLevel (string level)
    {//Opens level based on which level was chosen in ChooseLevel
        level = chosenLevel;
        Debug.Log(level);
        SceneManager.LoadScene(level);
    }
}
