using UnityEngine;
using UnityEngine.Events;

public class ItemSocket : MonoBehaviour
{
    [Header("Starting Item (Optional)")]
    [Tooltip("Ziehe hier das AITerminal rein, damit es beim Spielstart fest im Socket sitzt!")]
    public CarryableItem startingItem;

    [Header("Socket References")]
    public Transform insertPoint;
    public bool isOccupied = false;
    public CarryableItem currentItem;

    [Header("Socket Events")]
    public UnityEvent onActivated;
    public UnityEvent onDeactivated;

    private void Start()
    {
        // Wenn ein Start-Item zugewiesen ist -> beim Start direkt einsetzen:
        if (startingItem != null)
        {
            isOccupied = false;
            InsertItem(startingItem);
        }
    }

    public void InsertItem(CarryableItem item)
    {
        if (isOccupied || item == null) return;

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