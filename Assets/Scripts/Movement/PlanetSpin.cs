using UnityEngine;

public class PlanetSpin : MonoBehaviour {

    [SerializeField] public float SpinTime = 60f;

    private float speed;

    public void Start()
    {
        speed = 360f / SpinTime;
    }

    public void FixedUpdate()
    {
        
        transform.Rotate(transform.up, speed * Time.fixedDeltaTime, Space.World);
    }

    public Vector3 GetSurfaceVelocity(Vector3 point)
    {
        // Geschwindigkeit in Bogenmaß (Radians) umwandeln
        float speedRad = speed * Mathf.Deg2Rad;

        // Rotationsachse * Geschwindigkeit
        Vector3 angularVelocity = transform.up * speedRad;

        // Vektor vom Planeten-Zentrum zum Spieler
        Vector3 radiusVector = point - transform.position;

        // Das Kreuzprodukt ergibt exakt die vektorielle Bewegungsrichtung auf der Oberfläche
        return Vector3.Cross(angularVelocity, radiusVector);
    }
}
