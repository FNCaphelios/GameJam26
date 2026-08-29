using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform targetPoint;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public bool isMoving = false;

    private Transform currentPlanet;

    public void Start()
    {
        PlanetGravity planetGravity = GetComponentInParent<PlanetGravity>();
        if (planetGravity == null ) planetGravity = FindAnyObjectByType<PlanetGravity>();
        if ( planetGravity != null ) currentPlanet = planetGravity.transform;
    }

    public void Update()
    {
        if (!isMoving || targetPoint == null || currentPlanet == null) return;
        
        Vector3 planetUp = (transform.position - currentPlanet.position).normalized;
        transform.up = planetUp;

        Vector3 targetDirection = (targetPoint.position - transform.position).normalized;
        targetDirection = Vector3.ProjectOnPlane(targetDirection, planetUp).normalized;

        if(targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, planetUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
       
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            isMoving = false;
            Debug.Log("NPC has reached the target point.");
        }
    }

    public void WalkTo(Transform destination)
    {
        targetPoint = destination;
        isMoving = true;
    }
}
