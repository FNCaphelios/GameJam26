using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract :MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out NPCInteractable npcInteractable))
                {
                    npcInteractable.Interact();
                }

                if (collider.TryGetComponent(out TerminalInteractable terminalInteractable))
                {
                    terminalInteractable.Interact();
                }
            }
        }
    }
}
