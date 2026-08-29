using UnityEngine;

public class CarryableItem : MonoBehaviour
{
    [Header("Item Identifier")]
    public string itemName = "QuestItem"; // Match this in your NPCInteractable Inspector

    private Rigidbody rigidBody;
    private Collider collider;
    public bool isCarried = false;
    public ItemSocket currentSocket;

    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void PickUp(Transform holdPoint)
    {
        isCarried = true;

        if (currentSocket != null)
        {
            currentSocket.RemoveItem();
            currentSocket = null;
        }

        if (rigidBody != null) rigidBody.isKinematic = true;
        if (collider != null) collider.isTrigger = true;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isCarried = false;
        if (currentSocket != null)
        {
            currentSocket.RemoveItem();
            currentSocket = null;
        }
        Transform planetTransform = GetCurrentPlanetTransform();
        // get the player's forward direction and position for the drop origin
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        Vector3 forwardDir = (player != null) ? player.transform.forward : transform.forward;
        Vector3 playerPos = (player != null) ? player.transform.position : transform.position;
        Vector3 playerUp = (player != null) ? player.transform.up : Vector3.up;
        // start the drop origin a bit in front of the player and slightly above their feet
        Vector3 dropOrigin = playerPos + (forwardDir * 1.2f) + (playerUp * 0.5f);
        // direction to drop the item, defaulting to down if no planet is found
        Vector3 downDirection = Vector3.down;
        if (planetTransform != null)
        {
            downDirection = (planetTransform.position - dropOrigin).normalized;
        }
        // own the collider to avoid self-collision during the raycast:
        if (collider != null) collider.enabled = false;
        // Strahl vom Punkt VOR dem Spieler nach unten schießen:
        Ray ray = new Ray(dropOrigin, downDirection);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            // directly place the item on the hit point, slightly above the surface to avoid clipping:
            transform.position = hit.point + (hit.normal * 0.05f);
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else
        {
            // fallback: if no hit, place the item at the drop origin
            transform.position = dropOrigin;
        }
        // acivate the collider again and set it to non-trigger for physics interactions:
        if (collider != null)
        {
            collider.enabled = true;
            collider.isTrigger = false;
        }
        // place the item in the hierarchy under the planet (if any) to maintain relative positioning:
        transform.SetParent(planetTransform);
        if (rigidBody != null)
        {
            rigidBody.isKinematic = (planetTransform != null);
            rigidBody.linearVelocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.useGravity = (planetTransform == null);
        }
    }

    public void PlaceInSocket(ItemSocket socket, Transform socketPoint)
    {
        isCarried = false;
        currentSocket = socket;

        if (rigidBody != null) rigidBody.isKinematic = true;
        if (collider != null) collider.isTrigger = false;

        transform.SetParent(socketPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private Transform GetCurrentPlanetTransform()
    {
        GravityReceiver gravityReceiver = FindAnyObjectByType<GravityReceiver>();
        if(gravityReceiver != null && gravityReceiver.DominantPlanet != null) 
        {
            return gravityReceiver.DominantPlanet.transform;
        }

        PlanetGravity closetPlanet = null;
        float minDistance = float.MaxValue;
        foreach (PlanetGravity planet in FindObjectsByType<PlanetGravity>())
        {
            float distance = Vector3.Distance(transform.position, planet.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closetPlanet = planet;
            }
        }
        if (closetPlanet != null) return closetPlanet.transform;

        return null;
    }
}