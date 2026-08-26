using UnityEngine;

public class SunSystemLogic : MonoBehaviour
{
    [SerializeField] private GameObject planetA;
    [SerializeField] private GameObject planetB;

    [SerializeField] private float DistancePlanetA = 5000f;
    [SerializeField] private float DistancePlanetB = 7000f;
    [SerializeField] private float orbitSpeedPlanetA = 10f;
    [SerializeField] private float orbitSpeedPlanetB = 10f;


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
        if (planetA != null)
        {
            planetA.transform.RotateAround(transform.position, Vector3.up, orbitSpeedPlanetA * Time.fixedDeltaTime);
        }

        if (planetB != null)
        {
            planetB.transform.RotateAround(transform.position, Vector3.up, orbitSpeedPlanetB * Time.fixedDeltaTime);
        }
    }

    public Vector3 GetOrbitVelocity(Transform targetPlanet)
    {
        if (targetPlanet == planetA.transform)
        {
            float speedRad = orbitSpeedPlanetA * Mathf.Deg2Rad;
            Vector3 angularVelocity = Vector3.up * speedRad;
            Vector3 radiusVector = targetPlanet.position - transform.position;

            return Vector3.Cross(angularVelocity, radiusVector);
        }

        return Vector3.zero;
    }
}
