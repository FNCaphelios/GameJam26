using UnityEngine;

public class TerminalInteractable : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log("terminal opened");
        if(aiTerminal.Instance != null) aiTerminal.Instance.OpenTerminal();
    }
}
