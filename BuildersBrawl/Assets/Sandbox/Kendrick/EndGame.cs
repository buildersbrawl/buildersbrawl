using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionFunction
{
    public static T GetComponentInChildren<T>(this GameObject gameObject, int index)
    {
        return gameObject.transform.GetChild(index).GetComponent<T>();
    }
}

public class EndGame : MonoBehaviour
{
    //Keeps track of the number of players in the game
    public int[] players;
    public GameObject P1Data, P2Data, P3Data, P4Data;

    private bool P4 = true;
    private bool P3, P2 = false;

    public int title = 0;
    public int kills = 1;
    public int builds = 2;
    public int points = 3;
    public int wins = 4;

    //Stores player colors
    public Material P1_Color, P2_Color, P3_Color, P4_Color;

    void Awake()
    {
        P1Data.GetComponent<Image>().color = P1_Color.color;
        P2Data.GetComponent<Image>().color = P2_Color.color;
        P3Data.GetComponent<Image>().color = P3_Color.color;
        P4Data.GetComponent<Image>().color = P4_Color.color;
        CheckPlayerNumbers();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DisplayPoints();
    }

    void CheckPlayerNumbers()
    {
        if (players.Length < 4)
        {
            DisablePlayer4();
        }
        if (players.Length < 3)
        {
            DisablePlayer3();
        }
    }

    void DisablePlayer4()
    {
        P4Data.GetComponentInChildren<Text>().text = "";
        P4Data.GetComponentInChildren<Text>(title).text = "";
        P4Data.GetComponentInChildren<Text>(kills).text = "";
        P4Data.GetComponentInChildren<Text>(builds).text = "";
        P4Data.GetComponentInChildren<Text>(points).text = "";
        P4Data.GetComponentInChildren<Text>(wins).text = "";
        P4Data.GetComponent<Image>().color = Color.gray;
        P4 = false;
        P3 = true;
    }

    void DisablePlayer3()
    {
        P3Data.GetComponentInChildren<Text>().text = "";
        P3Data.GetComponentInChildren<Text>(kills).text = "";
        P3Data.GetComponentInChildren<Text>(builds).text = "";
        P3Data.GetComponentInChildren<Text>(points).text = "";
        P3Data.GetComponentInChildren<Text>(wins).text = "";
        P3Data.GetComponent<Image>().color = Color.gray;
        P3 = false;
        P2 = true;
    }

    void DisplayPoints()
    {

    }
}
