using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public static WinUI S;

    public string sceneToRestart;
    //public Text winText;
    public Image winImage;
    public int winImageNum = -1;

    public Sprite[] winImagesArr;

    private void Start()
    {
        if(S == null)
        {
            S = this;
        }
        else
        {
            Destroy(this);
        }

        this.gameObject.SetActive(false);
    }

    /*private void SetWinText(string playerName)
    {
        winText.text = playerName + " Wins!";
    }

    public void Restart()
    {
        SceneManager.LoadScene(sceneToRestart);
    }*/

    private void Update()
    {
        if(winImageNum >= 0)
        {
            winImage.sprite = winImagesArr[winImageNum];
        }
        
        
    }
}
