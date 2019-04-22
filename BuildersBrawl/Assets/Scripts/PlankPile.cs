using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankPile : MonoBehaviour
{
    //when player walks up and activates pick up plank if hits this

    //generates plank

    //gives plank to player

    public GameObject[] plankPrefab; 

    public GameObject GeneratePlank(Vector3 newPlankSpawnPosition, Quaternion newPlankSpawnRotation)
    {
        GameObject newlyBirthedPlank;

        newlyBirthedPlank = Instantiate(plankPrefab[Random.Range(0, plankPrefab.Length)], newPlankSpawnPosition, newPlankSpawnRotation);

        newlyBirthedPlank.GetComponent<PlankManager>().PickUpSpawn();

        return newlyBirthedPlank;
    }

}
