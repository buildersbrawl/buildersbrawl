using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinState : MonoBehaviour
{
    string playerWhoWon;
    private bool someoneWon = false;
    

    public GameObject winUI;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if(winUI == null)
        {
            print("trying to set reference to winUI");
            winUI = GameObject.Find("WinUI");
        }
        winUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        print("Win trigger " + other.gameObject.name);

        print(other.gameObject.GetComponent<PlayerController>() != null);
        print(!GameManager.S.someoneWon);

        //if player touching and no one has won yet
        if (other.gameObject.GetComponent<PlayerController>() != null && (!GameManager.S.someoneWon))
        {
            //set the winner
            GameManager.S.winner = other.gameObject;

            GameObject.Find("Main Camera").GetComponent<CameraController>().winnerDetermined = true;

            //call win animation
            GameManager.S.winner.GetComponent<PlayerAnimation>().CallVictoryAnimation();

            playerWhoWon = other.gameObject.name;
            playerWhoWon = playerWhoWon.Replace("Prefab_P", "");
            //GameObject.Find("Main Camera").GetComponent<CameraController>().ZoomOnWinner();

            //add points to the winner
            //other.gameObject.GetComponent<FlashyPoints>().ShowPointsGained(other.gameObject.transform.position, other.gameObject.GetComponent<Points>().pointsForOtherSide);
            //other.gameObject.GetComponent<Points>().AddPointsForOtherSide();

            //turn on win UI
            //winUI.GetComponent<Text>().text = playerWhoWon + " Won! \n \n Hit B to Continue";
            winUI.SetActive(true);
            

            //make ability to move to MM available
            GameManager.S.someoneWon = true;
            Debug.Log("someone won = " + GameManager.S.someoneWon);

            //look at pole, stand next to, then trigger animation
        }
    }
}
