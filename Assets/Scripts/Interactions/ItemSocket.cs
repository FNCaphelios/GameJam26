using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class ItemSocket : MonoBehaviour
{
    public Transform insertPoint;
    public bool isOccupied = true;
    public UnityEvent onActivated;

    public void InsertItem(CarryableItem item)
    {
        if (isOccupied) return;

        isOccupied = true;
        Transform targetPos = insertPoint != null ? insertPoint : transform;
        item.PlaceInSocket(targetPos);

        Debug.Log($"Item {item.name} inserted into socket {name}.");
        onActivated?.Invoke();
    }
}
