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
    [Header("Run Target Settings")]
    [SerializeField] private Transform runTargetPoint; // Where the NPC should run to
    [SerializeField] private float runSpeed = 6f;      // NPC movement speed
    [SerializeField] private bool disableNPCOnArrival = false; // Option to hide NPC when reaching destination

    private bool wasItemDialogueActive = false; // Tracks if the item dialogue triggered
    [Header("UI References")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;    // NPC response text area
    [SerializeField] private TextMeshProUGUI[] optionSlots;   // Array of choice UI text slots

    [Header("Dialogue Tree")]
    [SerializeField] private List<DialogueNode> dialogueNodes = new List<DialogueNode>();

    [Header("Conditional Dialogue (Item Check)")]
    [SerializeField] private string requiredItemName = "QuestItem"; // Item required in hand
    [SerializeField] private int itemDialogueNodeID = 5;            // Node ID to trigger when item is held
    [SerializeField] private Transform playerHandSocket;            // Reference to player's hand transform

    [Header("Camera Alignment")]
    [SerializeField] private Transform npcFocusPoint;
    [SerializeField] private float focusSpeed = 5f;
    [SerializeField] private bool maanulsen = false;

    [Header("AI Terminal Unlock")]
    [SerializeField] private string npcName = "";

    private int currentNodeID = 0;
    private int selectedOptionIndex = 0;
    public bool isDialogueActive = false;
   
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
        // Schaltet den NPC sofort im AI-Terminal frei:
        if (aiTerminal.Instance != null)
        {
            string targetName = !string.IsNullOrEmpty(npcName) ? npcName : gameObject.name;
            aiTerminal.Instance.UnlockNPCByName(targetName);
        }
        playerCam = Camera.main;
        isDialogueActive = true;
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null) player.canMove = false;
        // Prüfe, ob der Spieler das Terminal in der Hand hält:
        if (IsPlayerHoldingItem(requiredItemName))
        {
            currentNodeID = itemDialogueNodeID;
            wasItemDialogueActive = true;
        }
        else
        {
            currentNodeID = 0;
            wasItemDialogueActive = false;
        }
        if (dialogueCanvas != null) dialogueCanvas.SetActive(true);
        DisplayCurrentNode();
    }

    private bool IsPlayerHoldingItem(string targetName)
    {
        PlayerInteract player = FindAnyObjectByType<PlayerInteract>();

        if (player != null && player.item != null)
        {
            // 1. Check CarryableItem.itemName variable
            if (!string.IsNullOrEmpty(player.item.itemName) &&
                player.item.itemName.Equals(targetName, System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // 2. Check GameObject name (handles case sensitivity & clones)
            if (player.item.gameObject.name.IndexOf(targetName, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
        }

        return false;
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
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null) player.canMove = true;
        currentNodeID = 0;
        selectedOptionIndex = 0;
        if (dialogueCanvas != null) dialogueCanvas.SetActive(false);

        if (maanulsen && wasItemDialogueActive)
        {
            NPCMovement movement = GetComponent<NPCMovement>();
            if (movement == null) movement = gameObject.AddComponent<NPCMovement>();
            if (runTargetPoint != null)
            {
                movement.WalkTo(runTargetPoint);
            }
            else
            {
                movement.isMoving = true;
            }
            PlayerInteraction playerInteraction = FindAnyObjectByType<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.hasSpokenWithM = true;
            }
        }
    }
}