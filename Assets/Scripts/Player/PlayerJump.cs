using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Zıplama Ayarları")]
    public float jumpForce = 14f;
    public float gravityScale = 3f;
    public float maxFallSpeed = -18f;

    [Header("Rocket Ayarları")]
    public float rocketDuration = 2.5f;
    public float rocketSpeed = 20f;
    public float averagePlatformSpacing = 4.5f;

    public ScoreManager scoreManager;
    public PlayerFeedback feedback;

    private Rigidbody2D rb;
    private Collider2D col;

    private bool rocketActive = false;
    private float rocketTimer = 0f;
    private float rocketStartY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        // Rocket aktifken yukarı doğru uç
        if (rocketActive)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rocketSpeed
            );

            rocketTimer -= Time.deltaTime;

            if (rocketTimer <= 0f)
            {
                EndRocket();
            }

            return;
        }

        // Maksimum düşme hızını sınırla
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                maxFallSpeed
            );

            if (feedback != null)
            {
                feedback.PlayBounce();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Sadece Ground veya Platform ile ilgileniyoruz
        if (!collision.gameObject.CompareTag("Ground") &&
            !collision.gameObject.CompareTag("Platform"))
        {
            return;
        }

        // Gerçekten platformun/zeminin ÜSTÜNE indi mi?
        bool landedOnTop = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                landedOnTop = true;
                break;
            }
        }

        // Üstüne inmediysen zıplama
        if (!landedOnTop)
        {
            return;
        }

        // OTOMATİK ZIPLAMA
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );

        // Platformdan puan al
        if (collision.gameObject.CompareTag("Platform") &&
            scoreManager != null)
        {
            Platform platformScript =
                collision.gameObject.GetComponent<Platform>();

            if (platformScript != null &&
                !platformScript.scored)
            {
                platformScript.scored = true;
                scoreManager.AddPoint();
            }
        }
    }

    public void GrantRocket()
    {
        if (rocketActive)
            return;

        rocketActive = true;
        rocketTimer = rocketDuration;
        rocketStartY = transform.position.y;

        rb.gravityScale = 0f;

        if (col != null)
        {
            col.isTrigger = true;
        }

        if (feedback != null)
        {
            feedback.PlayRocketStart();
        }
    }

    void EndRocket()
    {
        rocketActive = false;

        rb.gravityScale = gravityScale;

        if (col != null)
        {
            col.isTrigger = false;
        }

        float distanceTraveled =
            transform.position.y - rocketStartY;

        int platformsPassed =
            Mathf.FloorToInt(
                distanceTraveled / averagePlatformSpacing
            );

        if (scoreManager != null)
        {
            for (int i = 0; i < platformsPassed; i++)
            {
                scoreManager.AddPoint();
            }
        }

        if (feedback != null)
        {
            feedback.PlayRocketEnd();
        }
    }
}