using UnityEngine;

[System.Serializable]
public class NPCData
{
    public string npcName;
    public string description;
    public string lockedText = "This NPC is locked.";
    public bool isUnlocked = false;
}
