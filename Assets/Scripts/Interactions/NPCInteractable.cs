using UnityEngine;
using TMPro;

public class NPCInteractable : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Dialogue Content")]
    [TextArea(2, 5)]
    [SerializeField] private string[] dialogueLines; // Add multiple lines in the Inspector!

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool playerInRange = false;

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
        if (!playerInRange) return;

        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else
        {
            AdvanceDialogue();
        }
    }

    private void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        isDialogueActive = true;
        currentLineIndex = 0;

        if (dialogueCanvas != null) dialogueCanvas.SetActive(true);
        DisplayCurrentLine();
    }

    private void AdvanceDialogue()
    {
        currentLineIndex++;

        // Check if there are more lines left
        if (currentLineIndex < dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue(); // Closed canvas when reaching the end of the text array
        }
    }

    private void DisplayCurrentLine()
    {
        if (dialogueText != null)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        currentLineIndex = 0;

        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
    }
}