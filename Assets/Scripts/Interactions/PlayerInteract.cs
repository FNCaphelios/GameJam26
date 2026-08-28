using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public Transform holdPoint;
    public CarryableItem item;
    public float interactRange = 2.5f;

    private void Start()
    {
        //automatically create a hold point if it doesn't exist
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
        // tab to open/close the terminal
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleTerminal();
        }

        // esc to close the terminal
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (aiTerminal.Instance != null && aiTerminal.Instance.isOpen)
            {
                aiTerminal.Instance.CloseTerminal();
            }
        }

        //if ui open don't allow world interaction
        if (aiTerminal.Instance != null && aiTerminal.Instance.isOpen) return;

        // q to drop the item
        if (item != null && Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            item.Drop();
            item = null;
        }

        // e to interact with the world (pick up, insert, talk)
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            HandleWorldInteraction();
        }
    }

    // radius-based interaction with the AI terminal, depending on whether the player is holding the terminal or not
    private void ToggleTerminal()
    {
        if (aiTerminal.Instance == null) return;

        //close the terminal if it's already open
        if (aiTerminal.Instance.isOpen)
        {
            aiTerminal.Instance.CloseTerminal();
            return;
        }

        //if the player is holding the terminal, open it regardless of distance
        if (item != null)
        {
            aiTerminal.Instance.OpenTerminal();
            return;
        }

        //if the player is not holding the terminal, check if they are within range of the terminal in the world
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
                Debug.Log("to far");
            }
        }
    }

    // Handle interactions with the world, such as picking up items, inserting items into sockets, and talking to NPCs
    private void HandleWorldInteraction()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider col in colliderArray)
        {
            //place item in socket if the player is holding an item and the socket is not activated
            if (item != null && col.TryGetComponent(out ItemSocket socket))
            {
                if (!socket.isOccupied)
                {
                    socket.InsertItem(item);
                    item = null;
                    return;
                }
            }

            // pick up item if the player is not holding an item and the item is not being carried
            if (item == null && col.TryGetComponent(out CarryableItem carryable))
            {
                if (!carryable.isCarried)
                {
                    carryable.PickUp(holdPoint);
                    item = carryable;
                    return;
                }
            }

            //interact with NPC if the player is not holding an item and the NPC is interactable
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
        }
    }
}