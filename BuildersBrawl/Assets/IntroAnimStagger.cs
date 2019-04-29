﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimStagger : MonoBehaviour
{
    public PlayerAnimation playerAnimRef;

    private void OnTriggerEnter(Collider other)
    {
        //print("triggered");

        if(other.gameObject.GetComponent<CameraController>() != null)
        {
            playerAnimRef.CallIntroAnimation();

            print(playerAnimRef.gameObject.name + " intro.");

            this.gameObject.SetActive(false);
        }
    }
}
