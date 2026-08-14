using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public float crackDelay = 0.3f;
    public float fallDelay = 0.2f;
    public Color crackColor = Color.red;

    private bool triggered = false;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float adjustedCrackDelay;
    private float adjustedFallDelay;
    private float capturedSlowFactor = 1f; // YENİ: Break() içinde de kullanabilmek için field'a alındı

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.transform.position.y < transform.position.y) return;

        triggered = true;

        capturedSlowFactor = (SlowMotionManager.Instance != null) ? SlowMotionManager.Instance.CurrentFactor : 1f;
        adjustedCrackDelay = crackDelay / capturedSlowFactor;
        adjustedFallDelay = fallDelay / capturedSlowFactor;

        Invoke(nameof(Crack), 0f);
    }

    void Crack()
    {
        if (this == null || gameObject == null) return;
        if (sr != null) sr.color = crackColor;
        Invoke(nameof(Break), adjustedCrackDelay);
    }

    void Break()
    {
        if (this == null || gameObject == null) return;
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f * capturedSlowFactor; // YENİ: düşüş de slow motion'a uyuyor
        Invoke(nameof(Vanish), adjustedFallDelay);
    }

    void Vanish()
    {
        if (this == null || gameObject == null) return;
        Destroy(gameObject);
    }
}