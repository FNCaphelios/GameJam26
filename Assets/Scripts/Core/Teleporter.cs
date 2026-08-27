using System.Net;
using UnityEngine;


public class Teleporter : MonoBehaviour
{

    public Teleporter destinationTeleporter;
    public Transform spawnPoint;
    [SerializeField] private float teleportCooldown = 5f;

    void Update()
    {
        if (teleportCooldown > 0)
        {
            teleportCooldown -= Time.deltaTime;
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        
        if (teleportCooldown > 0) return;

        Debug.Log("Etwas hat den Teleporter berührt: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null && destinationTeleporter != null && destinationTeleporter.spawnPoint != null)
            {
                
                destinationTeleporter.teleportCooldown = 5f;

                
                rb.position = destinationTeleporter.spawnPoint.position;

                
                rb.linearVelocity = Vector3.zero;
            }
            else
            {
                Debug.LogWarning("Teleporter: Destination oder SpawnPoint fehlen!");
            }
        }
    }



}
