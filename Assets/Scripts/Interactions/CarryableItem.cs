using UnityEngine;

public class CarryableItem : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Collider collider;
    public bool isCarried = false;

    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void PickUp(Transform holdPoint)
    {
        isCarried = true;
        if (rigidBody != null) rigidBody.isKinematic = true;
        if (collider != null) collider.isTrigger = true;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isCarried = false;
        if (rigidBody != null) rigidBody.isKinematic = false;
        if (collider != null) collider.isTrigger = false;
        transform.SetParent(null);
    }

    public void PlaceInSocket (Transform socketPoint)
    {
        isCarried = false;
        if (rigidBody != null) rigidBody.isKinematic = true;
        if (collider != null) collider.isTrigger = false;

        transform.SetParent(socketPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
