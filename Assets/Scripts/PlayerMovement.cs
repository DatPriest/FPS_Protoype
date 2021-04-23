using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -15.81f;
    public float jumpHeight = 3f;
    public float sprintFactor = 2f; // Fallback Value
    public float dashMultiplier = 4f;   

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public KeyCode lastKey;
    public Event e;

    private Vector3 velocity;
    bool isGrounded;
    bool isSprinting = false;

    private Vector3 _prevPos;
    private Vector3 _currentPos;

    float deltaTime;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("GroundCheck").transform;
        groundMask = LayerMask.GetMask("Ground");
    }


    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        e = Event.current;
        deltaTime = Time.deltaTime;

        _prevPos = _currentPos;
        _currentPos = transform.position;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        GetPlayerInputs();
        CalculateGravity();

    }


    void GetPlayerInputs()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
            isSprinting = false;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (x != 0 || z != 0)
            CalculateMovement(x, z);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
            Dash(x, z);
    }

    void CalculateGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void CalculateMovement(float x, float z)
    {
        Vector3 move = transform.right * x + transform.forward * z;

        if (isSprinting)
            controller.Move(move * speed * sprintFactor * deltaTime);
        else
            controller.Move(move * speed * deltaTime);
    }

    void Dash(float x, float z)
    {
        controller.Move(transform.right * dashMultiplier + transform.forward * dashMultiplier);
        Camera.main.
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, (_currentPos - _prevPos).normalized * 10);
    }
}

