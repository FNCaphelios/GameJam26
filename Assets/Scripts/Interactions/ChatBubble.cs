using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class ChatBubble :MonoBehaviour
{
    public static void Create(Transform parent, Vector3 localPosition, string text)
    {
        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble, parent);
        chatBubbleTransform.localPosition = localPosition;
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);
        Destroy(chatBubbleTransform.gameObject, 4f);
    }
    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        Setup("Hello World! sksksksk sksksksk skskskskksslllsösösös");
    }
    private void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 offset = new Vector2(8f, 2f);
        backgroundSpriteRenderer.size = textSize + offset;

        Vector3 leftshift = new Vector3(-12f, 0f);
        backgroundSpriteRenderer.transform.localPosition = new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f) + leftshift;
    }
}
