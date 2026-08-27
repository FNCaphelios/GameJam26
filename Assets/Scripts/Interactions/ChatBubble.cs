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

    private void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(4f, 2f);
        Vector2 minSize = new Vector2(6f, 3f);

        Vector2 finalSize = new Vector2(
            Mathf.Max(textSize.x + padding.x, minSize.x),
            Mathf.Max(textSize.y + padding.y, minSize.y)
        );

        backgroundSpriteRenderer.size = finalSize;

        backgroundSpriteRenderer.transform.localPosition = Vector3.zero;

        textMeshPro.transform.localPosition = Vector3.zero;
    }
}
