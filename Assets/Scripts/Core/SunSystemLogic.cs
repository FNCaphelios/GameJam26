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
        //place Both planets, based on the distance
       if (planetA != null)
        {
            planetA.transform.RotateAround(transform.position, Vector3.up, orbitSpeedPlanetA * Time.fixedDeltaTime);
        }

        if (planetB != null)
        {
            planetB.transform.RotateAround(transform.position, Vector3.up, orbitSpeedPlanetB * Time.fixedDeltaTime);
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
}
