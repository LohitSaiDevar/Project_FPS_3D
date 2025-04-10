using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] float speed = 12;
    [SerializeField] float gravity = -9.87f * 2;
    [SerializeField] float jumpHeight = 3;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    Vector3 velocity = new Vector3(0,0,0);

    bool isGrounded;
    bool isMoving;

    Vector3 lastPos;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Reset the velocity
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        //To move the player
        Vector3 movement = transform.right * inputX + transform.forward * inputZ;

        controller.Move(speed * Time.deltaTime * movement);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (lastPos != transform.position && isGrounded)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        lastPos = transform.position;
    }
}
