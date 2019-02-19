using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinState : MonoBehaviour
{
    string playerWhoWon;

    public GameObject winUI;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if(winUI == null)
        {
            print("no reference to winUI");
        }
        winUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            playerWhoWon = other.gameObject.name;

            //turn on win UI
            winUI.GetComponent<Text>().text = playerWhoWon + " Won! \n \n Hit B to Restart";
            winUI.SetActive(true);

            //make ability to move to MM available
            GameManager.S.someoneWon = true;
        }
    }
}
