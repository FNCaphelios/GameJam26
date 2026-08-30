using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Visual Parts (Alle separaten Teile)")]
    [Tooltip("Ziehe hier alle 5 Teile deines Charakters hinein!")]
    public Transform[] visualParts;

    [Header("Movement Settings")]
    public Transform targetPoint;
    public float moveSpeed = 4f;
    public float rotationSpeed = 8f;
    public bool isMoving = false;

    private Transform currentPlanet;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    public void Start()
    {
        FindClosestPlanet();
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    private void FindClosestPlanet()
    {
        float minDistance = float.MaxValue;
        foreach (PlanetGravity p in FindObjectsByType<PlanetGravity>(FindObjectsSortMode.None))
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                currentPlanet = p.transform;
            }
        }
    }

    public void Update()
    {
        if (!isMoving || targetPoint == null)
        {
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            return;
        }

        if (currentPlanet == null) FindClosestPlanet();

        Vector3 targetPos = targetPoint.position;
        Debug.DrawLine(transform.position, targetPos, Color.green);

        // 1. Oben-Richtung an den Planeten anpassen:
        Vector3 planetUp = transform.up;
        if (currentPlanet != null)
        {
            planetUp = (transform.position - currentPlanet.position).normalized;
        }

        // 2. Richtung zum Ziel berechnen:
        Vector3 moveDir = (targetPos - transform.position);
        Vector3 flatForward = Vector3.ProjectOnPlane(moveDir, planetUp);

        // 3. Kapsel drehen:
        if (flatForward.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatForward.normalized, planetUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 4. Kapsel vorwärts bewegen:
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // 5. ALLE 5 TEILE SYNCHRON MITBEWEGEN:
        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(lastRotation);

        if (visualParts != null && visualParts.Length > 0)
        {
            foreach (Transform part in visualParts)
            {
                if (part != null)
                {
                    // Verschiebt und dreht jeden Teil millimetergenau im Verbund:
                    Vector3 relativeOffset = part.position - lastPosition;
                    part.position = transform.position + (deltaRotation * relativeOffset);
                    part.rotation = deltaRotation * part.rotation;
                }
            }
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;

        // 6. Ziel erreicht?
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            isMoving = false;
            Debug.Log("<color=green>" + name + " hat das Ziel erreicht!</color>");
        }
    }

    public void WalkTo(Transform destination)
    {
        targetPoint = destination;
        FindClosestPlanet();
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        isMoving = true;
        Debug.Log("<color=cyan>" + name + " startet WalkTo zu: " + destination.name + "</color>");
    }
}