using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Prüft, ob das Objekt, das den Würfel berührt, der Spieler ist
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // Sucht den EndScreenManager und startet die Sequenz
            EndScreenManager endManager = FindAnyObjectByType<EndScreenManager>();
            if (endManager != null)
            {
                endManager.TriggerEnd();
            }
        }
    }
}