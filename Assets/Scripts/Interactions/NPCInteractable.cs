using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    private bool playerInRange;

    private void Start()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("Dialogue Canvas is NOT assigned on " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered NPC trigger range!");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left NPC trigger range!");
            playerInRange = false;
            EndDialogue();
        }
    }

    public void Interact()
    {
        if (playerInRange)
        {
            Debug.Log("Interact called successfully!");
            StartDialogue();
        }
        else
        {
            Debug.LogWarning("Interact called, but player is NOT in range!");
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
