using UnityEngine;

public class SunSystemLogic : MonoBehaviour
{
    [SerializeField] private GameObject planetA;
    [SerializeField] private GameObject planetB;

    [SerializeField] private float DistancePlanetA = 5000f;
    [SerializeField] private float DistancePlanetB = 7000f;
    [SerializeField] private float orbitSpeedPlanetA = 0.5f;
    [SerializeField] private float orbitSpeedPlanetB = 0.5f;

    void Start()
    {
        if (planetA != null)
        {
            Vector3 direction = (planetA.transform.position - transform.position).normalized;
            if (direction == Vector3.zero) direction = Vector3.right;
            planetA.transform.position = transform.position + (direction * DistancePlanetA);
        }

        if (planetB != null)
        {
            Vector3 direction = (planetB.transform.position - transform.position).normalized;
            if (direction == Vector3.zero) direction = Vector3.right;
            planetB.transform.position = transform.position + (direction * DistancePlanetB);
        }
    }

    void FixedUpdate()
    {
        // Wir steuern die Planeten jetzt über echte Physik statt Teleportation
        if (planetA != null) MovePlanetPhysically(planetA, orbitSpeedPlanetA);
        if (planetB != null) MovePlanetPhysically(planetB, orbitSpeedPlanetB);
    }

    /// <summary>
    /// Berechnet die Umlaufbahn und die Eigendrehung in einem einzigen, fehlerfreien Physik-Frame.
    /// Verhindert das "Bouncen" des Spielers durch sanftes Schieben (MovePosition) statt Teleportation.
    /// </summary>
    private void MovePlanetPhysically(GameObject planet, float orbitSpeed)
    {
        Rigidbody rb = planet.GetComponent<Rigidbody>();
        if (rb == null) return;

        // 1. Orbit-Rotation um die Sonne berechnen
        Quaternion orbitRot = Quaternion.AngleAxis(orbitSpeed * Time.fixedDeltaTime, Vector3.up);

        // 2. Neue Position berechnen
        Vector3 relativePos = rb.position - transform.position;
        Vector3 newPos = transform.position + (orbitRot * relativePos);

        // 3. Eigen-Spin des Planeten abfragen (falls das Skript aktiv ist)
        PlanetSpin spinScript = planet.GetComponent<PlanetSpin>();
        Quaternion spinRot = Quaternion.identity;
        if (spinScript != null && spinScript.SpinTime > 0)
        {
            float speed = 360f / spinScript.SpinTime;
            spinRot = Quaternion.AngleAxis(speed * Time.fixedDeltaTime, rb.transform.up);
        }

        // 4. Perfekte physikalische Bewegung anwenden
        rb.MovePosition(newPos);
        // Die neue Rotation ist: Die Drehung um die Sonne + die Eigendrehung des Planeten
        rb.MoveRotation(orbitRot * rb.rotation * spinRot);
    }

    public Vector3 GetOrbitVelocity(Transform targetPlanet, Vector3 playerPosition)
    {
        if (targetPlanet == planetA.transform)
        {
            float speedRad = orbitSpeedPlanetA * Mathf.Deg2Rad;
            Vector3 angularVelocity = Vector3.up * speedRad;
            Vector3 radiusVector = playerPosition - transform.position;
            return Vector3.Cross(angularVelocity, radiusVector);
        }
        else if (targetPlanet == planetB.transform)
        {
            float speedRad = orbitSpeedPlanetB * Mathf.Deg2Rad;
            Vector3 angularVelocity = Vector3.up * speedRad;
            Vector3 radiusVector = playerPosition - transform.position;
            return Vector3.Cross(angularVelocity, radiusVector);
        }

        return Vector3.zero;
    }
}