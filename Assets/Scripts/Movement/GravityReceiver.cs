using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityReceiver : MonoBehaviour
{
    private PlanetGravity planet;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Sucht sich für diesen Test automatisch den Planeten in der Szene
        planet = FindAnyObjectByType<PlanetGravity>();
    }

    void FixedUpdate()
    {
        if (planet != null)
        {
            planet.Attract(rb);
        }
    }
}