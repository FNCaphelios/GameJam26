using UnityEngine;
using TMPro;
using System.Collections; 
public class EndScreenManager : MonoBehaviour
{
    [Header("Test Toggle")]
    [Tooltip("Setze diesen Haken im Play-Mode, um das Ende zu testen!")]
    public bool triggerEndScreen = false;
    private bool hasTriggered = false;

    [Header("UI Referenzen")]
    public GameObject blackBackground;
    public TextMeshProUGUI theEndText;
    public TextMeshProUGUI thanksText;

    [Header("Timing")]
    public float initialDelay = 1f; // Pause, bevor der erste Text kommt
    public float delayBetweenTexts = 2.5f; // Pause zwischen Text 1 und Text 2

    void Start()
    {
        // Zu Spielstart zur Sicherheit alles verstecken
        if (blackBackground != null) blackBackground.SetActive(false);
        if (theEndText != null) theEndText.gameObject.SetActive(false);
        if (thanksText != null) thanksText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Prüft, ob du den Haken im Inspector gesetzt hast
        if (triggerEndScreen && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(PlayEndSequence());
        }
    }

    // --- Diese Methode kannst du später von anderen Skripten aus aufrufen ---
    // Beispiel: FindAnyObjectByType<EndScreenManager>().TriggerEnd();
    public void TriggerEnd()
    {
        triggerEndScreen = true;
    }

    // Die zeitliche Abfolge (Coroutine)
    IEnumerator PlayEndSequence()
    {
        // 1. Schwarzen Hintergrund aktivieren
        blackBackground.SetActive(true);

        // 2. Kurz warten (wirkt filmischer, als wenn der Text sofort da ist)
        yield return new WaitForSeconds(initialDelay);

        // 3. "The End" einblenden
        theEndText.gameObject.SetActive(true);

        // 4. Die eingestellte Zeit warten...
        yield return new WaitForSeconds(delayBetweenTexts);

        // 5. "thanks for playing!" einblenden
        thanksText.gameObject.SetActive(true);

        // Optional für später: Hier könnte man den Cursor wieder freigeben, 
        // damit der Spieler das Spiel beenden kann.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}