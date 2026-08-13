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

        // Platformun üstüne gelmediyse çalışmasın.
        bool landedOnTop = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                landedOnTop = true;
                break;
            }
        }

        if (!landedOnTop) return;

        triggered = true;

        Invoke(nameof(Crack), 0f);
    }

    void Crack()
    {
        if (this == null || gameObject == null) return;

        if (sr != null)
        {
            sr.color = crackColor;
        }

        float delay = crackDelay;

        if (SlowMotionManager.Instance != null)
        {
            delay = SlowMotionManager.Instance.GetDelay(crackDelay);
        }

        Invoke(nameof(Break), delay);
    }

    void Break()
    {
        if (this == null || gameObject == null) return;

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;

        float delay = fallDelay;

        if (SlowMotionManager.Instance != null)
        {
            delay = SlowMotionManager.Instance.GetDelay(fallDelay);
        }

        Invoke(nameof(Vanish), delay);
    }

    void Vanish()
    {
        if (this == null || gameObject == null) return;

        Destroy(gameObject);
    }
}