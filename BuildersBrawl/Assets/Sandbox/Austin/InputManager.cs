using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public ControllerAssigner controllerAssignerInstance;

    [Range(0f, 1f)]
    public float triggerSensitivity = 0.5f;

    public int gamePlayerId = 0;
    //private Player player;

    private Controller activeController;

    //direction player is moving
    public Vector3 moveVector;

    public bool isMoving = false;
    public bool isJumping = false;
    public bool isCharging = false;
    public bool isPushing = false;
    public bool isUsingBoard = false;
    public bool isSlamming = false;
    public bool moveUIUp = false;
    public bool moveUILeft = false;
    public bool moveUIRight = false;
    public bool moveUIDown = false;

    public static bool isUsingUI = true;
    public bool isMovingUI = false;

    private Scene currentScene;
    private string sceneName = "";
    public bool controllerSelected = false;

    private Rewired.Player player { get { return PlayerSelect.GetRewiredPlayer(gamePlayerId); } }


    void Start()
    {

        

        /*currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if(sceneName == "Main_Menu" || sceneName == "ControllerSelectScreen")
        {
            isUsingUI = true;
        }*/
    }

    void Update()
    {
        if (!ReInput.isReady)
            return;
        if (player == null)
            return;

        GetInput();
        ProcessInput();

        //Debug.Log(controllerSelected);
    }

     

    void GetInput()
    {
        //give player a playerID
        //player = ReInput.players.GetPlayer(playerID);
        //Debug.Log("Using UI in GETINPUT() "+ isUsingUI);

        /*if (isUsingUI)
        {
                controllerSelected = player.GetButtonDown("Submit");
                moveUIDown = player.GetButtonDown("UIDown");
                moveUILeft = player.GetButtonDown("UILeft");
                moveUIRight = player.GetButtonDown("UIRight");
                moveUIUp = player.GetButtonDown("UIUp");

                moveVector.x = player.GetAxis("UIHorizontal");
                moveVector.z = player.GetAxis("UIVertical");
                moveVector.y = 0f;
                if (moveVector != Vector3.zero)
                    isMovingUI = true;
            
        }*/
        if(!isUsingUI)
        {
          
            Debug.Log("INPUT FOR GAME");
            //input for left stick
            moveVector.x = player.GetAxis("Move Horizontal");
            moveVector.y = player.GetAxis("Move Vertical");
            moveVector.z = 0f;
            if (moveVector != Vector3.zero)
                isMoving = true;
            
            //input for A, B, X, Y
            isJumping = player.GetButtonDown("Jump");
            isCharging = player.GetButtonDown("Charge");
            isPushing = player.GetButtonDown("Push");
            isUsingBoard = player.GetButtonDown("Use Board");

            //input for bumpbers
            isSlamming = player.GetButtonDown("Board Slam");
        
            //if trigger is pressed beyond triggerSensitivity it will trigger the slam
            if (player.GetAxis("Board Slam") >= triggerSensitivity)
                isSlamming = true;

        }
        

        
    }

    void ProcessInput()
    {
        if (isMoving)
            Debug.Log("Moving " + moveVector.x + " in the X direction and " + moveVector.z + " in the Z direction");

        if (isJumping)
            Debug.Log("Jump");

        if (isCharging)
            Debug.Log("Charge");

        if (isPushing)
            Debug.Log("Push");

        if (isUsingBoard)
            Debug.Log("Using Board");

        if (isSlamming)
            Debug.Log("Slam");

        if (moveUIDown)
            Debug.Log("UIDOWN");
        if (moveUIUp)
            Debug.Log("UIUP");
        if (moveUILeft)
            Debug.Log("UILEFT");
        if (moveUIRight)
            Debug.Log("UIRight");

        if (controllerSelected)
            Debug.Log("UISubmit");
        if (isMovingUI)
            Debug.Log("UIMoving");
    }

    public void ChangeControllerForGame(Rewired.Player rewiredPlayer)
    {
        rewiredPlayer.controllers.maps.SetMapsEnabled(false, "UI");
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");
    }

    public void ChangeControllerForUI(Rewired.Player rewiredPlayer)
    {
        rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Default");
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "UI");
        
    }

    public Image ChangeToRed(Image toChange)
    {
        if (gamePlayerId == 0)
        {
            toChange.color = UnityEngine.Color.red;
        }
        else if (gamePlayerId == 1)
        {
            toChange.color = UnityEngine.Color.red;
        }
        return toChange;
    }
}
