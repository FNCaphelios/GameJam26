using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [Header("Was soll gespawnt werden?")]
    public GameObject[] prefabs;

    [Header("Verteilung")]
    public int amountToSpawn = 100;

    [Tooltip("Sollte deutlich größer sein als der höchste Berg auf dem Planeten")]
    public float raycastStartDistance = 20f;

    [Tooltip("Der Layer deines Planeten. Verhindert, dass Bäume auf anderen Bäumen spawnen.")]
    public LayerMask planetLayer;

    public float minSpacing = 1.5f;

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = amountToSpawn * 10;

        while (spawned < amountToSpawn && attempts < maxAttempts)
        {
            attempts++;

            // 1. Eine zufällige Richtung auf der Kugel generieren
            Vector3 randomDirection = Random.onUnitSphere;

            // 2. Weit OBERHALB des Planeten (über den höchsten Bergen) anfangen
            Vector3 rayStartPos = transform.position + (randomDirection * raycastStartDistance);

            // 3. Den Strahl vom Startpunkt in Richtung des Planetenmittelpunkts feuern
            Ray ray = new Ray(rayStartPos, -randomDirection);
            RaycastHit hit;

            // 4. Prüfen, ob der Strahl das Planeten-Mesh trifft
            if (Physics.Raycast(ray, out hit, raycastStartDistance * 2f, planetLayer))
            {
                // hit.point ist nun die exakte Höhe an dieser Stelle des Meshes
                Vector3 spawnPos = hit.point;

                // 5. Abstands-Check
                if (!Physics.CheckSphere(spawnPos, minSpacing))
                {
                    // 6. Baum ausrichten: hit.normal ist die exakte Neigung des Bodens an diesem Pixel!
                    // Das heißt, der Baum wächst auch an steilen Hängen senkrecht aus dem Hang heraus.
                    Quaternion spawnRot = Quaternion.FromToRotation(Vector3.up, hit.normal);

                    GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
                    GameObject newObject = Instantiate(randomPrefab, spawnPos, spawnRot, transform);

                    float randomScale = Random.Range(0.8f, 1.2f);
                    newObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

                    spawned++;
                }
            }
        }
    }
}