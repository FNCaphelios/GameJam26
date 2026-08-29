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

            NPCInteractable npc = hit.collider.GetComponent<NPCInteractable>();

            // --- BEDINGUNG 1: NPC ---
            if (npc != null && !npc.isDialogueActive)
            {
                interactionText.text = "Press E to interact";

                
            }

            // --- BEDINGUNG 2: Item ---
            else if (hit.collider.CompareTag("Item"))
            {

                if (hit.collider.CompareTag("Item") && hasSpokenWithM == true)
                {
                    interactionText.text = "Press E to pickup";
                }
                else
                {
                    interactionText.text = "Press Tab to open Terminal";
                }


            }
            
            }
        }
    
}