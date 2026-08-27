using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float gravityStrength = 9.81f;

    public Vector3 Attract(Rigidbody body)
    {
        // body.position = Rigidbody of the Player
        // transform.position = Object of the Script (Planet)
        // Player - Planet Vector
        Vector3 distanceV = body.position - transform.position;

        // Player-Planet distance
        float distanceSquared = distanceV.sqrMagnitude;

        if (distanceSquared < 0.0001f) return Vector3.zero;


        float currentGravity = gravityStrength / distanceSquared;
        Vector3 gravityUp = distanceV.normalized;
        Vector3 appliedForce = -gravityUp * currentGravity;

        // Gravity force for the Player 
        // gravity Strength = Gravity
        // ForceMode.Acceleration: implements Force directly onto the physical velocity. !Mass of Body is unimportant atm!
        //body.AddForce(-gravityUp * gravityStrength, ForceMode.Acceleration);
        body.AddForce(appliedForce, ForceMode.Acceleration);

        return appliedForce;
    }
}