using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityReceiver : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Planets")]
    [SerializeField] private PlanetGravity planetA;
    [SerializeField] private PlanetGravity planetB;

    [Header("Debug Gravity Forces")]
    [SerializeField] private float forceFromPlanetA;
    [SerializeField] private float forceFromPlanetB;
    [SerializeField] private float totalForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 gravityA = Vector3.zero;
        Vector3 gravityB = Vector3.zero;

        // check gravity and applay
        if (planetA != null) gravityA = planetA.Attract(rb);
        if (planetB != null) gravityB = planetB.Attract(rb);

        // for debugging, shows the current gravity
        forceFromPlanetA = gravityA.magnitude;
        forceFromPlanetB = gravityB.magnitude;
        totalForce = forceFromPlanetA + forceFromPlanetB;

        Vector3 netGravity = gravityA + gravityB;

        if (netGravity.sqrMagnitude > 0.001f)
        {
           
            Vector3 gravityUp = -netGravity.normalized;
            Vector3 localUp = rb.transform.up;

            Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * rb.rotation;
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 50f * Time.fixedDeltaTime));
        }
    }
}