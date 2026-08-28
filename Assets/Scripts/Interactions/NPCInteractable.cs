using UnityEngine;
using TMPro;

public class NPCInteractable : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Dialogue Content")]
    [TextArea(2, 5)]
    [SerializeField] private string[] dialogueLines;

    [Header("Camera Alignment")]
    [SerializeField] private Transform npcFocusPoint; // Drag the NPC's Head/Eyes transform here
    [SerializeField] private float focusSpeed = 5f;

    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private Camera playerCam;
    private Quaternion originalCamRotation;

    private void Start()
    {
        if (dialogueCanvas != null) dialogueCanvas.SetActive(false);
    }

    private void Update()
    {
        // Smoothly look at the NPC focus point while dialogue is open
        if (isDialogueActive && playerCam != null && npcFocusPoint != null)
        {
            Vector3 direction = npcFocusPoint.position - playerCam.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, targetRotation, Time.deltaTime * focusSpeed);
        }
    }

    public void Interact()
    {
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

        playerCam = Camera.main;
        if (playerCam != null)
        {
            originalCamRotation = playerCam.transform.rotation;
        }

        isDialogueActive = true;
        currentLineIndex = 0;

        if (dialogueCanvas != null) dialogueCanvas.SetActive(true);
        DisplayCurrentLine();
    }

    private void AdvanceDialogue()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
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