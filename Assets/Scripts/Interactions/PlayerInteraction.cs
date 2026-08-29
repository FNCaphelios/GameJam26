using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; 

public class PlayerInteraction : MonoBehaviour
{
    [Header("Einstellungen")]
    public float interactionDistance = 3f;
    public bool hasSpokenWithM = false;

    [Header("Referenzen")]
    public Camera playerCamera;
    public TextMeshProUGUI interactionText;

    void Update()
    {
        interactionText.text = "";

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // --- BEDINGUNG 1: NPC ---
            if (hit.collider.CompareTag("NPC"))
            {
                interactionText.text = "Press E to interact";

                // NEU: Abfrage über das New Input System
                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Debug.Log("Du interagierst mit dem NPC!");
                    // hasSpokenWithM = true;
                }
            }
            // --- BEDINGUNG 2: Item ---
            else if (hit.collider.CompareTag("Item") && hasSpokenWithM == true)
            {
                interactionText.text = "Press E to pickup";

                // NEU: Abfrage über das New Input System
                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Debug.Log("Item aufgehoben!");
                    // hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }
}