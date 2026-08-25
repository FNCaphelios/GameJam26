using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float gravityStrength = 9.81f;

    public void Attract(Rigidbody body)
    {
        // 1. Richtungsvektoren bestimmen
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 localUp = body.transform.up;

        // 2. Gravitation anwenden (zieht den Spieler an)
        body.AddForce(-gravityUp * gravityStrength, ForceMode.Acceleration);

        // 3. Spieler aufrichten, sodass die Füße zum Planeten zeigen
        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;

        // Slerp sorgt für eine weiche Drehung, damit die Kamera nicht ruckelt
        body.MoveRotation(Quaternion.Slerp(body.rotation, targetRotation, 50f * Time.fixedDeltaTime));
    }
}