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

    [Header("Double Jump Ayarları")]
    public int extraJumps = 0;

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

        // Oyun başladığında Player'ın ilk zıplamasını garanti et.
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void Update()
    {
        // =========================
        // ROCKET
        // =========================
        if (rocketActive)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rocketSpeed);

            rocketTimer -= Time.deltaTime;

            if (rocketTimer <= 0f)
            {
                EndRocket();
            }

            return;
        }

        // =========================
        // MAX FALL SPEED
        // =========================
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                maxFallSpeed
            );
        }

        // =========================
        // DOUBLE JUMP
        // =========================
        if (extraJumps > 0 && Input.GetMouseButtonDown(0))
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            extraJumps--;

            if (feedback != null)
            {
                feedback.PlayDoubleJump();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Sadece Ground veya Platform ile ilgileniyoruz.
        if (!collision.gameObject.CompareTag("Ground") &&
            !collision.gameObject.CompareTag("Platform"))
        {
            return;
        }

        // Gerçekten platformun ÜSTÜNE indiğimizi kontrol et.
        bool landedOnTop = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                landedOnTop = true;
                break;
            }
        }

        // Platformun yanına veya altına çarptıysak zıplama.
        if (!landedOnTop)
        {
            return;
        }

        // =========================
        // NORMAL OTOMATİK ZIPLAMA
        // =========================
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );

        // =========================
        // SKOR
        // =========================
        if (collision.gameObject.CompareTag("Platform") &&
            scoreManager != null)
        {
            Platform platformScript =
                collision.gameObject.GetComponent<Platform>();

            if (platformScript != null && !platformScript.scored)
            {
                platformScript.scored = true;
                scoreManager.AddPoint();
            }
        }

        // =========================
        // FEEDBACK
        // =========================
        if (feedback != null)
        {
            feedback.PlayBounce();
        }
    }

    // =========================
    // ROCKET
    // =========================

    public void GrantRocket()
    {
        if (rocketActive)
            return;

        rocketActive = true;
        rocketTimer = rocketDuration;
        rocketStartY = transform.position.y;

        rb.gravityScale = 0f;

        // Rocket sırasında platformlarla fiziksel çarpışmayı kapat.
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

    // =========================
    // DOUBLE JUMP
    // =========================

    public void GrantDoubleJump()
    {
        extraJumps = 1;
    }
}