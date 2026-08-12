using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public float crackDelay = 0.3f;
    public float fallDelay = 0.2f;
    public Color crackColor = Color.red;

    private bool triggered = false;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.transform.position.y < transform.position.y) return; // alttan geldiyse yoksay

        triggered = true;
        Invoke(nameof(Crack), 0f);
    }

    void Crack()
    {
        if (this == null || gameObject == null) return;
        if (sr != null) sr.color = crackColor;
        Invoke(nameof(Break), crackDelay);
    }

    void Break()
    {
        if (this == null || gameObject == null) return;
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        Invoke(nameof(Vanish), fallDelay);
    }

    void Vanish()
    {
        if (this == null || gameObject == null) return;
        Destroy(gameObject);
    }
}