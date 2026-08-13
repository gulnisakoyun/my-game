using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float riseSpeed = 1.5f;
    public float lifetime = 1f;

    private float elapsed = 0f;
    private TextMeshPro textMesh;

    public static void Create(string message, Vector3 position, Color color)
    {
        GameObject go = new GameObject("FloatingText");
        go.transform.position = position;

        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.text = message;
        tmp.fontSize = 4;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;

        go.AddComponent<FloatingText>();
    }

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        if (textMesh != null)
        {
            Color c = textMesh.color;
            c.a = Mathf.Lerp(1f, 0f, elapsed / lifetime);
            textMesh.color = c;
        }

        if (elapsed >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}