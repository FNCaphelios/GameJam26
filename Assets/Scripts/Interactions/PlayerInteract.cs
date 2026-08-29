using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public Transform holdPoint;
    public CarryableItem item;
    public float interactRange = 2.5f;

    private void Start()
    {
        // Automatically create a hold point if it doesn't exist
        if (holdPoint == null)
        {
            Camera cam = GetComponentInChildren<Camera>();
            if (cam != null)
            {
                Transform existing = cam.transform.Find("HoldPoint");
                if (existing != null)
                {
                    holdPoint = existing;
                }
                else
                {
                    GameObject hp = new GameObject("HoldPoint");
                    hp.transform.SetParent(cam.transform);
                    hp.transform.localPosition = new Vector3(0.25f, -0.25f, 0.5f);
                    hp.transform.localRotation = Quaternion.identity;
                    holdPoint = hp.transform;
                }
            }
        }
    }

    private void Update()
    {
        // Tab to open/close the terminal
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleTerminal();
        }

        // Esc to close the terminal
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (aiTerminal.Instance != null && aiTerminal.Instance.isOpen)
            {
                aiTerminal.Instance.CloseTerminal();
            }
        }

        // If UI open don't allow world interaction
        if (aiTerminal.Instance != null && aiTerminal.Instance.isOpen) return;

        // Q to drop the item
        if (item != null && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            item.Drop();
            item = null;
        }
        // E to interact with the world (pick up, insert, talk)
        else if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                HandleWorldInteraction();
            }


    }

    // Radius-based interaction with the AI terminal, depending on whether the player is holding the terminal or not
    private void ToggleTerminal()
    {
        if (aiTerminal.Instance == null) return;

        // Close the terminal if it's already open
        if (aiTerminal.Instance.isOpen)
        {
            aiTerminal.Instance.CloseTerminal();
            return;
        }

        // If the player is holding the terminal, open it regardless of distance
        if (item != null)
        {
            aiTerminal.Instance.OpenTerminal();
            return;
        }

        // If the player is not holding the terminal, check if they are within range of the terminal in the world
        CarryableItem worldTerminal = FindAnyObjectByType<CarryableItem>();
        if (worldTerminal != null)
        {
            float distance = Vector3.Distance(transform.position, worldTerminal.transform.position);
            if (distance <= interactRange)
            {
                aiTerminal.Instance.OpenTerminal();
            }
            else
            {
                Debug.Log("too far");
            }
        }
    }

    // Handle interactions with the world, such as picking up items, inserting items into sockets, and talking to NPCs
    private void HandleWorldInteraction()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider col in colliderArray)
        {
            // 1. Place item in socket if holding an item and socket is empty
            if (item != null && col.TryGetComponent(out ItemSocket socket))
            {
                if (!socket.isOccupied)
                {
                    socket.InsertItem(item);
                    item = null;
                    return;
                }
            }

            // 2. Interact with NPC (Works regardless of holding an item or not)
            NPCInteractable npc = col.GetComponent<NPCInteractable>();
            if (npc == null)
            {
                npc = col.GetComponentInParent<NPCInteractable>();
            }

            if (npc != null)
            {
                npc.Interact();
                return;
            }

            // 3. Pick up item if not currently holding an item
            if (item == null && col.TryGetComponent(out CarryableItem carryable))
            {
                if (!carryable.isCarried)
                {
                    carryable.PickUp(holdPoint);
                    item = carryable;
                    return;
                }
            }
        }
    }
}