﻿using System.Collections;
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
            print("trying to set reference to winUI");
            winUI = GameObject.Find("WinUI");
        }
        winUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if player touching and no one has won yet
        if (other.gameObject.GetComponent<PlayerController>() != null && (!GameManager.S.someoneWon))
        {
            playerWhoWon = other.gameObject.name;
            playerWhoWon = playerWhoWon.Replace("Prefab_P", "");

            //add points to the winner
            other.gameObject.GetComponent<Points>().AddPointsForOtherSide();

            //turn on win UI
            winUI.GetComponent<Text>().text = playerWhoWon + " Won! \n \n Hit B to Restart";
            winUI.SetActive(true);

            //make ability to move to MM available
            GameManager.S.someoneWon = true;
        }
    }
}
