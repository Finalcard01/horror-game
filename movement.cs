using System;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class movement : MonoBehaviour
{
    [Header("movement")]
    public float movespeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    [Header("keybinds")]
    public KeyCode jumpKey = KeyCode.Space;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    UnityEngine.Vector3 moveDirection;

    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        SpeedControl();
        grounded = Physics.Raycast(transform.position, UnityEngine.Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //when to jump
        if (Input.GetKey(jumpKey) && grounded && readyToJump)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MovePlayer()
    {// calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * movespeed * 10f, ForceMode.Force);
            //on air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * movespeed * 10f * airMultiplier, ForceMode.Force);
    }
    private void SpeedControl()
    {
        UnityEngine.Vector3 flatVel = new UnityEngine.Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > movespeed)
        {
            UnityEngine.Vector3 limitedVel = flatVel.normalized * movespeed;
            rb.linearVelocity = new UnityEngine.Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new UnityEngine.Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
