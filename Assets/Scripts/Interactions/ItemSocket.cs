using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class ItemSocket : MonoBehaviour
{
    public Transform insertPoint;
    public bool isOccupied = true;
    public CarryableItem currentItem;
    public UnityEvent onActivated;
    public UnityEvent onDeactivated;

    public void InsertItem(CarryableItem item)
    {
        if (isOccupied) return;

        isOccupied = true;
        currentItem = item;
        Transform targetPos = insertPoint != null ? insertPoint : transform;
        item.PlaceInSocket(this, targetPos);

        Debug.Log($"Item {item.name} inserted into socket {name}.");
        onActivated?.Invoke();
    }

    public void RemoveItem()
    {
        isOccupied = false;
        currentItem = null;
        Debug.Log($"Item removed from socket {name}.");
        onDeactivated?.Invoke();
    }
}
