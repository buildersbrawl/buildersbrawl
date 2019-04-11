using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameInputManager : MonoBehaviour
{
    //public ControllerAssigner controllerAssignerInstance;

    [Range(0f, 1f)]
    public float triggerSensitivity = 0.5f;

    public int gamePlayerId = 0;
    //private Player player;

    private Controller activeController;

    //joystick input
    public Vector3 joystickInput;

    [HideInInspector]
    public bool joystickIsInputing = false;
    [HideInInspector]
    public bool pressedJumpButton = false;
    [HideInInspector]
    public bool pressedChargeButton = false;
    [HideInInspector]
    public bool pressedPushButton = false;
    [HideInInspector]
    public bool pressedBoardPickUpOrDropButton = false;
    [HideInInspector]
    public bool pressedSlamButton = false;
    [HideInInspector]
    public bool moveUIUp = false;
    [HideInInspector]
    public bool moveUILeft = false;
    [HideInInspector]
    public bool moveUIRight = false;
    [HideInInspector]
    public bool moveUIDown = false;

    public static bool isUsingUI = true;
    public bool isMovingUI = false;

    private Scene currentScene;
    private string sceneName = "";
    public bool controllerSelected = false;

    [SerializeField]
    private Rewired.Player player { get { return PlayerSelect.GetRewiredPlayer(gamePlayerId); } }


    void Start()
    {
        isUsingUI = false;
        //Rewired.Player player = PlayerSelect.GetRewiredPlayer(gamePlayerId);
        if(player == null)
        {
            print("Input isn't set up, start in controller select");
            return;
        }
        print("Player is " + player.id);

        /*currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if(sceneName == "Main_Menu" || sceneName == "ControllerSelectScreen")
        {
            isUsingUI = true;
        }*/
    }

    public void Init(PlayerController.PlayerNumber pNum)
    {
        if(pNum == PlayerController.PlayerNumber.p1)
        {
            gamePlayerId = PlayerSelect.S.p1Controller;
        }
        else
        {
            //p2
            gamePlayerId = PlayerSelect.S.p2Controller;
        }
    }

    void FixedUpdate()
    {
        if (!ReInput.isReady)
            return;
        if (player == null)
            return;

        GetInput();
        //ProcessInput();

        //Debug.Log(controllerSelected);
    }



    void GetInput()
    {

        //print("isUsingUI is " + isUsingUI);

        //print("Getting Input player " + player.id);
        //give player a playerID
        //player = ReInput.players.GetPlayer(playerID);
        //Debug.Log("Using UI in GETINPUT() "+ isUsingUI);

        
        if (!isUsingUI)
        {
            //print("not using UI player " + player.id);
            //Debug.Log("INPUT FOR GAME");
            //input for left stick
            joystickInput.x = player.GetAxis("Move Horizontal");
            joystickInput.z = player.GetAxis("Move Vertical");
            joystickInput.y = 0f;
            if (joystickInput != Vector3.zero)
                joystickIsInputing = true;

            //input for A, B, X, Y
            
            pressedJumpButton = player.GetButtonDown("Jump");
            if (pressedJumpButton)
            {
                print("Player " + player.id + "hit a");
            }

            pressedChargeButton = player.GetButtonDown("Charge");
            pressedPushButton = player.GetButtonDown("Slam/Push");
            pressedBoardPickUpOrDropButton = player.GetButtonDown("Use Board");

            //input for bumpbers
            pressedSlamButton = player.GetButtonDown("Slam/Push");

            //if trigger is pressed beyond triggerSensitivity it will trigger the slam
            if (player.GetAxis("Slam/Push") >= triggerSensitivity)
                pressedSlamButton = true;

        }



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
    /*

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
    */
}
