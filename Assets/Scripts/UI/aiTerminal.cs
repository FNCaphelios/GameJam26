using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class aiTerminal : MonoBehaviour 
{
    public static aiTerminal Instance;

    [Header("UI elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Button Images")]
    public Image[] buttonImages;

    [Header("npc data")]
    public NPCData[] npcs = new NPCData[4];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // unlock the cursor and make it visible when the terminal is opened
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateButtonsVisuals();
        SelectNPC(0); // Select the first NPC by default
    }

    // Ensure that the button visuals are updated in the editor when changes are made
    private void OnValidate()
    {
        if (buttonImages != null && buttonImages.Length > 0)
        {
            UpdateButtonsVisuals();
        }
    }

    // Update the button visuals based on the NPCs' unlocked status
    public void UpdateButtonsVisuals()
    {
        for (int i = 0; i < buttonImages.Length; i++)
        {
            if (i < npcs.Length && buttonImages[i] != null)
            {
               if (npcs[i].isUnlocked)
                {
                    buttonImages[i].color = Color.white;
                }
               else
                {
                    buttonImages[i].color = Color.black;
                }
            }
            else
            {
                buttonImages[i].sprite = null; // No sprite for this button
                buttonImages[i].color = Color.clear; // Make the button invisible
            }
        }
    }

    // Select an NPC and update the UI with its information
    public void SelectNPC(int index)
    {
        if (index >= 0 && index < npcs.Length)
        {
            NPCData npc = npcs[index];

            if (npc.isUnlocked)
            {
                if (nameText != null) nameText.text = npc.npcName;
                if (descriptionText != null) descriptionText.text = npc.description;
            }

            else
            {
                if (nameText != null) nameText.text = npc.npcName;
                if (descriptionText != null) descriptionText.text = npc.lockedText;
            }
        }
    }

    // Unlock an NPC and update the button visuals
    public void UnlockNPC(int index)
    {
        if(index >= 0 && index < npcs.Length)
        {
            npcs[index].isUnlocked = true;
            UpdateButtonsVisuals();
            SelectNPC(index);
        }
    }
}