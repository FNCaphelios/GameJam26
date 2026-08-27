using UnityEngine;

public class NPCInteractable :MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    private bool playerInRange; // Fixed: now used below!

    private void Start()
    {
        if (dialogueCanvas != null) dialogueCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            EndDialogue();
        }
    }

    public void Interact()
    {
        // Only open dialogue if the player is actually in range
        if (playerInRange)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(true);
        }
    }

    public void EndDialogue()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
    }
}

