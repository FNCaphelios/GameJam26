using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class aiTerminal : MonoBehaviour 
{
    public static aiTerminal Instance;

    [Header("The viewable terminal panel")]
    public GameObject terminalPanel;

    [Header("UI elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Button Images")]
    public Image[] buttonImages;

    [Header("Audio")]
    public AudioClip music;

    [Header("npc data")]
    public NPCData[] npcs = new NPCData[4];

    [Header("Intro Dialog Panel")]
    public GameObject introDialogPanel;
    public bool hasSeenIntroDialog = false;

    public bool isOpen = false;

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
    
    // sets panel to close on start
    private void Start()
    {
        CloseTerminal();
    }

    private void Update()
    {
        if (isOpen)
        {
            if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void OpenTerminal()
    {
        isOpen = true;
        if (terminalPanel != null) terminalPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null) player.canMove = false;

        if(!hasSeenIntroDialog && introDialogPanel != null)
        {
            introDialogPanel.SetActive(true);
            hasSeenIntroDialog = true;

            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusicOnceWithFade(music, 2f, 1f);
            }
        }
        else
        {
            if(introDialogPanel != null)
            {
                UpdateButtonsVisuals();
                SelectNPC(0);
            }
        }
    }

    public void FinishIntroDialog()
    {
        hasSeenIntroDialog = true;
        if (introDialogPanel != null) introDialogPanel.SetActive(false);

        UpdateButtonsVisuals();
        SelectNPC(0);
    }

    public void CloseTerminal()
    {
        isOpen = false;
        if (terminalPanel != null) terminalPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null) player.canMove = true;
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