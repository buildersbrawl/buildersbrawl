using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public string sceneToRestart;
    public Text winText;

    private void SetWinText(string playerName)
    {
        winText.text = playerName + " Wins!";
    }

    public void Restart()
    {
        SceneManager.LoadScene(sceneToRestart);
    }
}
