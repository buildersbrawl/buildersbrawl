using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneControls : MonoBehaviour
{
    public int playerNumber = 0;
    public GameObject RestartObject;
    public float speed = 10f;
    public float gravity = 1f;
    public Vector3 moveDirection;
    public Vector3 moveD;
    public CharacterController controller;
    public bool isMovingUp = false;
    public bool isMovingDown = false;
    public bool isMovingLeft = false;
    public bool isMovingRight = false;
    public bool isRigidbody = false;
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        moveDirection = RestartObject.transform.position;
        //if (!isRigidbody)
        //    controller = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeathObject")
            isRigidbody = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRigidbody)
        {
            moveD.y -= (gravity * Time.deltaTime);
            controller.Move(moveD);
        }
        else { }
            

        /*if (isRigidbody)
        {
            if (playerNumber == 0)
            {
                isMovingUp = Input.GetKey("w");
                isMovingDown = Input.GetKey("s");
                isMovingLeft = Input.GetKey("a");
                isMovingRight = Input.GetKey("d");
            }
            else if (playerNumber == 1)
            {
                isMovingUp = Input.GetKey(KeyCode.UpArrow);
                isMovingDown = Input.GetKey(KeyCode.DownArrow);
                isMovingLeft = Input.GetKey(KeyCode.LeftArrow);
                isMovingRight = Input.GetKey(KeyCode.RightArrow);
            }

            if (isMovingUp)
                transform.position += Vector3.forward * speed * Time.deltaTime;
            if (isMovingDown)
                transform.position += Vector3.back * speed * Time.deltaTime;
            if (isMovingLeft)
                transform.position += Vector3.left * speed * Time.deltaTime;
            if (isMovingRight)
                transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            Debug.Log("hello " + controller.isGrounded);
            if (playerNumber == 0)
            {
                Debug.Log("in player 0");
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    moveDirection.z += (speed * Time.deltaTime);
                    
                }
                    
                if (Input.GetKey(KeyCode.DownArrow))
                    moveDirection.z -= (speed * Time.deltaTime);
                if (Input.GetKey(KeyCode.RightArrow))
                    moveDirection.x += (speed * Time.deltaTime);
                if (Input.GetKey(KeyCode.LeftArrow)) { }
                    moveDirection.x -= (speed * Time.deltaTime);

                //moveDirection = new Vector3(Input.GetButton(, 0.0f, Input.GetAxis("Vertical"));
                //moveDirection = transform.TransformDirection(moveDirection);
                //moveDirection *= speed;
            }
            else if(playerNumber == 1) //(controller.isGrounded &&
            {
                Debug.Log("in player 1");
                if (Input.GetKey("a"))
                    moveDirection.z += (speed * Time.deltaTime);
                if (Input.GetKey("s"))
                    moveDirection.z -= (speed * Time.deltaTime);
                if (Input.GetKey("d"))
                    moveDirection.x += (speed * Time.deltaTime);
                if (Input.GetKey("a"))
                    moveDirection.x -= (speed * Time.deltaTime);

                //moveDirection = new Vector3(Input.GetButton(, 0.0f, Input.GetAxis("Vertical"));
                //moveDirection = transform.TransformDirection(moveDirection);
                //moveDirection *= speed;
            }

            if (!controller.isGrounded)
            {
                moveDirection.y -= (gravity * Time.deltaTime);
                Debug.Log("Not Grounded");
            }
            else
            {
                Debug.Log("Grounded");
            }

            controller.Move(moveDirection * Time.deltaTime);
            //Debug.Log(moveDirection);
    }*/
        
    }
}
