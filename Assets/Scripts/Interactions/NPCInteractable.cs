using UnityEngine;

public class NPCInteractable :MonoBehaviour
{
    public void Interact()
    {
        ChatBubble.Create(transform.transform, new Vector3(-0.5f, 3f, 0f), "Hello");
    }
}
