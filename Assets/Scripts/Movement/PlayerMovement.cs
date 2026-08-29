using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GravityReceiver))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;

    public float lookSpeed = 0.5f;
    public float lookXLimit = 45f;
    private float rotationX = 0;
    private float mouseXDelta = 0f;

    public float jumpForce = 5f;
    public float airControlAcceleration = 15f;

    [Header("Ground Check Settings")]
    [Tooltip("Der Radius der Kugel, die den Boden prüft.")]
    public float groundCheckRadius = 0.3f;
    [Tooltip("Verschiebt die Prüfkugel nach oben/unten. Pass dies im Scene-Fenster an, bis die grüne Kugel genau deine Füße umschließt!")]
    public float groundCheckOffset = -0.1f;

    [Tooltip("Wähle hier alle Layer aus, die als Boden gelten. Der Layer des Spielers darf NICHT ausgewählt sein!")]
    public LayerMask groundMask;

    [SerializeField] private bool isGrounded;
    private Vector3 referenceFrameVelocity = Vector3.zero;

    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction sprintAction;

    private Rigidbody rb;
    private GravityReceiver gravityReceiver;
    public bool canMove = true;

    // --- JUMP LOGIK (Buffer & Toleranz) ---
    private float jumpBufferCounter = 0f;
    private float groundedCounter = 0f;
    private float jumpBufferTime = 0.2f;
    private float coyoteTime = 0.15f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravityReceiver = GetComponent<GravityReceiver>();

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

        HandleCameraLook();

        if (jumpAction.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        Vector3 trueUp = gravityReceiver.GravityUp != Vector3.zero ? gravityReceiver.GravityUp : transform.up;

        UpdateReferenceFrame(trueUp);
        ApplyRigidbodyRotation(trueUp);

        if (isGrounded)
        {
            groundedCounter = coyoteTime;
        }
        else
        {
            groundedCounter -= Time.fixedDeltaTime;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input.sqrMagnitude > 1f) input.Normalize();

        Vector3 moveDirection = (transform.forward * input.y) + (transform.right * input.x);
        moveDirection = Vector3.ProjectOnPlane(moveDirection, trueUp).normalized;

        float currentSpeed = sprintAction.IsPressed() ? sprintSpeed : walkSpeed;
        Vector3 targetHorizontalVelocity = moveDirection * currentSpeed;

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 relativeVelocity = currentVelocity - referenceFrameVelocity;

        float currentVerticalSpeed = Vector3.Dot(relativeVelocity, trueUp);
        Vector3 currentHorizontalVelocity = relativeVelocity - (trueUp * currentVerticalSpeed);

        if (isGrounded)
        {
            currentHorizontalVelocity = targetHorizontalVelocity;
        }
        else
        {
            currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, targetHorizontalVelocity, airControlAcceleration * Time.fixedDeltaTime);
        }

        if (jumpBufferCounter > 0f && groundedCounter > 0f)
        {
            currentVerticalSpeed = jumpForce;
            jumpBufferCounter = 0f;
            groundedCounter = 0f;
        }

        rb.linearVelocity = currentHorizontalVelocity + (trueUp * currentVerticalSpeed) + referenceFrameVelocity;
    }

    private void HandleCameraLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        rotationX -= lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        mouseXDelta += lookInput.x * lookSpeed;
    }

    private void ApplyRigidbodyRotation(Vector3 trueUp)
    {
        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, trueUp).normalized;
        if (flatForward == Vector3.zero) flatForward = transform.forward;

        Quaternion uprightRotation = Quaternion.LookRotation(flatForward, trueUp);
        Quaternion yawRotation = Quaternion.AngleAxis(mouseXDelta, trueUp);

        rb.MoveRotation(yawRotation * uprightRotation);
        mouseXDelta = 0f;
    }

    private void UpdateReferenceFrame(Vector3 trueUp)
    {
        // Absolute Position der Prüf-Kugel bestimmen
        Vector3 checkPosition = transform.position + (trueUp * groundCheckOffset);

        // NEU: OverlapSphere statt SphereCast. Prüft simpel und verlässlich, ob in diesem Bereich Boden ist.
        Collider[] hitColliders = Physics.OverlapSphere(checkPosition, groundCheckRadius, groundMask);

        isGrounded = hitColliders.Length > 0;

        PlanetSpin activePlanetSpin = null;
        Transform activePlanetTransform = null;

        if (isGrounded)
        {
            // Wir nehmen den ersten getroffenen Collider (Boden), um die Planetendaten abzufragen
            activePlanetSpin = hitColliders[0].GetComponentInParent<PlanetSpin>();
            if (activePlanetSpin != null)
            {
                activePlanetTransform = activePlanetSpin.transform;
            }
        }
        else if (gravityReceiver != null && gravityReceiver.DominantPlanet != null)
        {
            activePlanetTransform = gravityReceiver.DominantPlanet.transform;
            activePlanetSpin = activePlanetTransform.GetComponent<PlanetSpin>();
        }

        Vector3 spinVelocity = Vector3.zero;
        Vector3 orbitVelocity = Vector3.zero;

        if (activePlanetTransform != null)
        {
            if (activePlanetSpin != null)
            {
                spinVelocity = activePlanetSpin.GetSurfaceVelocity(transform.position);
            }

            SunSystemLogic sunSystem = FindAnyObjectByType<SunSystemLogic>();
            if (sunSystem != null)
            {
                orbitVelocity = sunSystem.GetOrbitVelocity(activePlanetTransform, transform.position);
            }
        }

        referenceFrameVelocity = spinVelocity + orbitVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 currentUp = transform.up;
        if (Application.isPlaying)
        {
            GravityReceiver gr = GetComponent<GravityReceiver>();
            if (gr != null && gr.GravityUp != Vector3.zero) currentUp = gr.GravityUp;
        }

        Vector3 checkPosition = transform.position + (currentUp * groundCheckOffset);

        Gizmos.color = isGrounded ? Color.green : Color.red;

        // Zeichnet nur noch EINE Blase, die genau den Überprüfungsbereich darstellt
        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
    }
}