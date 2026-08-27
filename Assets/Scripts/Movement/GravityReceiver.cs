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
    public PlanetGravity DominantPlanet { get; private set; }

    // Neuer öffentlicher Vektor, damit das Movement-Skript weiß, wo "oben" ist
    public Vector3 GravityUp { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 gravityA = Vector3.zero;
        Vector3 gravityB = Vector3.zero;

        if (planetA != null) gravityA = planetA.Attract(rb);
        if (planetB != null) gravityB = planetB.Attract(rb);

        float forceA = gravityA.magnitude;
        float forceB = gravityB.magnitude;

        
        

        if (planetA != null && planetB != null)
            DominantPlanet = forceA > forceB ? planetA : planetB;
        else if (planetA != null) DominantPlanet = planetA;
        else if (planetB != null) DominantPlanet = planetB;
        else DominantPlanet = null;


        forceFromPlanetA = gravityA.magnitude;
        forceFromPlanetB = gravityB.magnitude;
        totalForce = forceFromPlanetA + forceFromPlanetB;

        Vector3 netGravity = gravityA + gravityB;

        if (netGravity.sqrMagnitude > 0.001f)
        {
            GravityUp = -netGravity.normalized;
        }
        else
        {
            GravityUp = Vector3.zero;
        }
    }
}