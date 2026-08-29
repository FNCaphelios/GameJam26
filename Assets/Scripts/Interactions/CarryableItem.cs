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

        if (rigidBody != null) rigidBody.isKinematic = false;
        if (collider != null) collider.isTrigger = false;
        transform.SetParent(null);
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
}