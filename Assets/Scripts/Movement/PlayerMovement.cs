using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;

    public float lookSpeed = 0.5f;
    public float lookXLimit = 45f;
    private float rotationX = 0;
    public float jumpForce = 5f;
    [SerializeField] private bool isGrounded;
    public float groundCheckDistance = 1.1f;
    private Vector3 referenceFrameVelocity = Vector3.zero;

    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction sprintAction;

    private Rigidbody rb;
    private bool canMove = true;
    private bool wantsToJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!canMove) return;

        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        rotationX -= lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed, 0);

        // Ground Check mit 'out hit'
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance);

        if (isGrounded)
        {
            // Planetenrotation auslesen, falls vorhanden
            PlanetSpin planetSpin = hit.collider.GetComponentInParent<PlanetSpin>();
            if (planetSpin != null)
            {
                referenceFrameVelocity = planetSpin.GetSurfaceVelocity(transform.position);
            }
            else
            {
                referenceFrameVelocity = Vector3.zero;
            }

            if (jumpAction.WasPressedThisFrame())
            {
                wantsToJump = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        Vector3 moveDirection = (transform.forward * input.y) + (transform.right * input.x);
        float currentSpeed = sprintAction.IsPressed() ? sprintSpeed : walkSpeed;
        Vector3 desiredMoveVelocity = moveDirection * currentSpeed;
        float currentVerticalSpeed = Vector3.Dot(rb.linearVelocity, transform.up);

        if (wantsToJump)
        {
            currentVerticalSpeed = jumpForce;
            wantsToJump = false;
        }

        // Die referenceFrameVelocity wird hier am Ende addiert
        rb.linearVelocity = desiredMoveVelocity + (transform.up * currentVerticalSpeed) + referenceFrameVelocity;
    }
}