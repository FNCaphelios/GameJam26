using UnityEngine;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public struct PlayerChoice
{
    [TextArea(2, 4)]
    public string choiceText;   // Text shown to the player
    public int nextNodeID;      // ID of the node to jump to (-1 ends dialogue)
}

[System.Serializable]
public class DialogueNode
{
    public int nodeID;                  // Unique identifier for this step
    [TextArea(3, 6)]
    public string npcResponseText;     // What the NPC says at this node
    public List<PlayerChoice> choices;  // Choices available to the player
}

public class NPCInteractable : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;    // NPC response text area
    [SerializeField] private TextMeshProUGUI[] optionSlots;   // Array of choice UI text slots

    [Header("Dialogue Tree")]
    [SerializeField] private List<DialogueNode> dialogueNodes = new List<DialogueNode>();

    [Header("Camera Alignment")]
    [SerializeField] private Transform npcFocusPoint;
    [SerializeField] private float focusSpeed = 5f;

    private int currentNodeID = 0;
    private int selectedOptionIndex = 0;
    private bool isDialogueActive = false;
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

        // Navigate options using keyboard
        DialogueNode currentNode = GetNodeByID(currentNodeID);
        if (currentNode != null && currentNode.choices.Count > 0)
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
        else
        {
            DialogueNode currentNode = GetNodeByID(currentNodeID);

            // If there are options to choose, pick the current selection
            if (currentNode != null && currentNode.choices.Count > 0)
            {
                SelectOption(selectedOptionIndex);
            }
            else
            {
                // Terminal node (no choices left) — press E to exit
                EndDialogue();
            }
        }
    }

    private void StartDialogue()
    {
        if (dialogueNodes == null || dialogueNodes.Count == 0) return;

        playerCam = Camera.main;
        isDialogueActive = true;
        currentNodeID = 0; // Starts at Node 0 (Opening Line)

        if (dialogueCanvas != null) dialogueCanvas.SetActive(true);

        DisplayCurrentNode();
    }

    private void DisplayCurrentNode()
    {
        DialogueNode currentNode = GetNodeByID(currentNodeID);

        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        selectedOptionIndex = 0;

        // 1. Set NPC Text
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);
            dialogueText.text = currentNode.npcResponseText;
        }

        // 2. Display Choice Slots
        for (int i = 0; i < optionSlots.Length; i++)
        {
            if (i < currentNode.choices.Count)
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
        DialogueNode currentNode = GetNodeByID(currentNodeID);
        if (currentNode == null || currentNode.choices.Count == 0) return;

        selectedOptionIndex = Mathf.Clamp(selectedOptionIndex + direction, 0, currentNode.choices.Count - 1);

        for (int i = 0; i < currentNode.choices.Count; i++)
        {
            UpdateOptionVisual(i);
        }
    }

    private void UpdateOptionVisual(int index)
    {
        if (index >= optionSlots.Length) return;

        DialogueNode currentNode = GetNodeByID(currentNodeID);
        if (currentNode == null || index >= currentNode.choices.Count) return;

        // Highlight active item with Outer Wilds yellow style
        if (index == selectedOptionIndex)
        {
            optionSlots[index].text = $"> <color=#FF9900>{currentNode.choices[index].choiceText}</color>";
        }
        else
        {
            optionSlots[index].text = $"  <color=#CCCCCC>{currentNode.choices[index].choiceText}</color>";
        }
    }

    private void SelectOption(int index)
    {
        DialogueNode currentNode = GetNodeByID(currentNodeID);
        if (currentNode == null || index >= currentNode.choices.Count) return;

        int nextID = currentNode.choices[index].nextNodeID;

        if (nextID < 0)
        {
            EndDialogue();
        }
        else
        {
            currentNodeID = nextID;
            DisplayCurrentNode();
        }
    }

    private DialogueNode GetNodeByID(int id)
    {
        return dialogueNodes.Find(node => node.nodeID == id);
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        currentNodeID = 0;
        selectedOptionIndex = 0;

        if (dialogueCanvas != null) dialogueCanvas.SetActive(false);
    }
}