using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Zıplama Ayarları")]
    public float jumpForce = 14f;
    public float gravityScale = 3f;
    public float maxFallSpeed = -18f; // negatif değer! çok hızlı düşmeyi engeller

    public ScoreManager scoreManager;
    public PlayerFeedback feedback;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);

            if (feedback != null)
            {
                feedback.PlayBounce();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (collision.gameObject.CompareTag("Platform") && scoreManager != null)
            {
                Platform platformScript = collision.gameObject.GetComponent<Platform>();

                if (platformScript != null && !platformScript.scored)
                {
                    platformScript.scored = true;
                    scoreManager.AddPoint();
                }
            }
        }
    }
}