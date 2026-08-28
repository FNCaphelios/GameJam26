using UnityEngine;
using TMPro;

[System.Serializable]
public struct DialogueOption
{
    public string optionText;       // The text choice shown to the player
    [TextArea(2, 4)]
    public string npcResponse;      // What the NPC says after selecting this option
}

public class NPCInteractable : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;    // NPC response text area
    [SerializeField] private TextMeshProUGUI[] optionSlots;   // Array of 2–4 choice UI text slots

    [Header("Dialogue Content")]
    [SerializeField] private DialogueOption[] dialogueOptions;

    [Header("Camera Alignment")]
    [SerializeField] private Transform npcFocusPoint;
    [SerializeField] private float focusSpeed = 5f;

    private int selectedOptionIndex = 0;
    private bool isDialogueActive = false;
    private bool showingResponse = false;
    private Camera playerCam;

    private void Start()
    {
        if (dialogueCanvas != null) dialogueCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!isDialogueActive) return;

        // Smooth camera look target
        if (playerCam != null && npcFocusPoint != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(npcFocusPoint.position - playerCam.transform.position);
            playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, targetRotation, Time.deltaTime * focusSpeed);
        }

        // Navigate options using keyboard when choices are displayed
        if (!showingResponse && dialogueOptions.Length > 0)
        {
            if (UnityEngine.InputSystem.Keyboard.current != null)
            {
                var kbd = UnityEngine.InputSystem.Keyboard.current;

                if (kbd.wKey.wasPressedThisFrame || kbd.upArrowKey.wasPressedThisFrame)
                {
                    ChangeSelection(-1);
                }
                else if (kbd.sKey.wasPressedThisFrame || kbd.downArrowKey.wasPressedThisFrame)
                {
                    ChangeSelection(1);
                }
            }
        }
    }

    public void Interact()
    {
        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else if (showingResponse)
        {
            // Second press of E returns to choice menu or ends dialogue
            EndDialogue();
        }
        else
        {
            // Pressing E selects the highlighted choice
            SelectOption(selectedOptionIndex);
        }
    }

    private void StartDialogue()
    {
        if (dialogueOptions == null || dialogueOptions.Length == 0) return;

        playerCam = Camera.main;
        isDialogueActive = true;
        showingResponse = false;
        selectedOptionIndex = 0;

        if (dialogueCanvas != null) dialogueCanvas.SetActive(true);

        DisplayOptions();
    }

    private void DisplayOptions()
    {
        if (dialogueText != null) dialogueText.text = "Select a question:";

        for (int i = 0; i < optionSlots.Length; i++)
        {
            if (i < dialogueOptions.Length)
            {
                optionSlots[i].gameObject.SetActive(true);
                UpdateOptionVisual(i);
            }
            else
            {
                optionSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void ChangeSelection(int direction)
    {
        selectedOptionIndex = Mathf.Clamp(selectedOptionIndex + direction, 0, dialogueOptions.Length - 1);

        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            UpdateOptionVisual(i);
        }
    }

    private void UpdateOptionVisual(int index)
    {
        if (index >= optionSlots.Length) return;

        // Add Outer Wilds style selection arrow [>] to highlighted item
        if (index == selectedOptionIndex)
        {
            optionSlots[index].text = $"> <color=#FF9900>{dialogueOptions[index].optionText}</color>";
        }
        else
        {
            optionSlots[index].text = $"  <color=#CCCCCC>{dialogueOptions[index].optionText}</color>";
        }
    }

    private void SelectOption(int index)
    {
        showingResponse = true;

        // Hide option choices
        foreach (var slot in optionSlots)
        {
            if (slot != null) slot.gameObject.SetActive(false);
        }

        // Display NPC's direct response to the selected option
        if (dialogueText != null)
        {
            dialogueText.text = dialogueOptions[index].npcResponse;
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        showingResponse = false;
        selectedOptionIndex = 0;

        if (dialogueCanvas != null) dialogueCanvas.SetActive(false);
    }
}