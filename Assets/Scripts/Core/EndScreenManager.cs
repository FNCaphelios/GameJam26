using UnityEngine;
using TMPro;
using System.Collections;

public class EndScreenManager : MonoBehaviour
{
    [Header("Test Toggle")]
    public bool triggerEndScreen = false;
    private bool hasTriggered = false;

    [Header("UI Referenzen")]
    public GameObject blackBackground;
    public TextMeshProUGUI descriptionText; // NEU: Der Beschreibungstext
    public TextMeshProUGUI theEndText;
    public TextMeshProUGUI thanksText;

    [Header("Timing")]
    public float initialDelay = 1f;
    public float descriptionDuration = 5f; // Wie lange man die Beschreibung lesen kann
    public float delayBeforeCredits = 1.5f; // Pause, wenn die Beschreibung verschwindet
    public float delayBetweenTexts = 2f;

    void Start()
    {
        if (blackBackground != null) blackBackground.SetActive(false);
        if (descriptionText != null) descriptionText.gameObject.SetActive(false);
        if (theEndText != null) theEndText.gameObject.SetActive(false);
        if (thanksText != null) thanksText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (triggerEndScreen && !hasTriggered)
        {
            TriggerEnd();
        }
    }

    public void TriggerEnd()
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            triggerEndScreen = true;
            StartCoroutine(PlayEndSequence());
        }
    }

    IEnumerator PlayEndSequence()
    {
        // 0. Spieler-Bewegung stoppen (greift auf dein bestehendes PlayerMovement-Skript zu)
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null) player.canMove = false;

        // 1. Bildschirm wird schwarz
        blackBackground.SetActive(true);
        yield return new WaitForSeconds(initialDelay);

        // 2. Beschreibung ("Der Astronaut fand, was er suchte...")
        descriptionText.gameObject.SetActive(true);
        yield return new WaitForSeconds(descriptionDuration);

        // 3. Beschreibung wieder ausblenden
        descriptionText.gameObject.SetActive(false);
        yield return new WaitForSeconds(delayBeforeCredits);

        // 4. Abspann ("The End")
        theEndText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delayBetweenTexts);

        // 5. "Thanks for playing"
        thanksText.gameObject.SetActive(true);

        // Mauszeiger wieder einblenden, falls man das Spiel danach schließen will
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}