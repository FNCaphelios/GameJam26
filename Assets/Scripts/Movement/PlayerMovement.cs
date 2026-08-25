using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float lookSpeed = 0.5f;
    public float lookXLimit = 45f;
    private float rotationX = 0;
    public float jumpForce = 5f;
    [SerializeField] private bool isGrounded;
    public float groundCheckDistance = 1.1f;

    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;

    private Rigidbody rb;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();

        // locks cursor in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!canMove) return;

        // up/down logic
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        rotationX -= lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // left/right logic
        transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed, 0);

        // Das 'bool' am Anfang ist weg!
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance);
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            // ForceMode.Impulse ist wichtig, da es ein sofortiger Stoß ist, keine stetige Kraft
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

    }

    void FixedUpdate()
    {
        if (!canMove) return;

        //reads Input meaning Keys (WASD). W/S are on the same axis. A/D on the same axis
        Vector2 input = moveAction.ReadValue<Vector2>();

        // without this, our character would move with 1.44f when going diagonally instead of 1.0f 
        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        // logic so that character always moves forward, depending on where the camera is facing
        Vector3 moveDirection = (transform.forward * input.y) + (transform.right * input.x);

        Vector3 targetVelocity = moveDirection * walkSpeed;
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;




    }
}