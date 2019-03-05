using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public void SelectPlayerOne()
    {
        //Selects player one if not previously done
        if (!PlayerSelect.S.playerOneSelected)
        {
            PlayerSelect.S.playerOneSelected = true;
            Debug.Log("Player One selected");
            PlayerSelect.S.CheckSelectedPlayers();
        }
    }

    public void SelectPlayerTwo()
    {
        //Selects player two if not previously done and if player one was already selected
        if (!PlayerSelect.S.playerTwoSelected && PlayerSelect.S.playerOneSelected)
        {
            PlayerSelect.S.playerTwoSelected = true;
            Debug.Log("Player Two selected");
            PlayerSelect.S.CheckSelectedPlayers();
        }
    }
}
