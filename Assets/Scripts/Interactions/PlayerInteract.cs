using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract :MonoBehaviour
{
    public Transform holdPoint;
    public CarryableItem item;

    private void Update()
    {
        if (aiTerminal.Instance != null && aiTerminal.Instance.isOpen) return;
        if (item != null && Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (aiTerminal.Instance != null)
            {
                aiTerminal.Instance.OpenTerminal();
            }
        }
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            float interactRange = 2.5f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider col in colliderArray)
            {
                if (item != null && col.TryGetComponent(out ItemSocket socket))
                {
                    if (!socket.isOccupied)
                    {
                        socket.InsertItem(item);
                        item = null; // Wir tragen es nicht mehr
                        return;
                    }
                }
                if (item == null && col.TryGetComponent(out CarryableItem carryable))
                {
                    if (!carryable.isCarried)
                    {
                        carryable.PickUp(holdPoint);
                        item = carryable;
                        return;
                    }
                }
                // C) Stehen wir vor einem NPC? -> Sprechen!
                if (col.TryGetComponent(out NPCInteractable npc))
                {
                    npc.Interact();
                    return;
                }
            }
        }
    }
}
