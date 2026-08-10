using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 7f;
    public float airControlMultiplier = 1f; // havadayken yön değiştirme gücü (1 = tam kontrol, 0.5 = zor kontrol)

    [Header("Ekran Sınırı")]
    public Camera mainCamera;

    private Rigidbody2D rb;
    private float touchInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float keyboardInput = Input.GetAxis("Horizontal");
        float moveInput = touchInput != 0f ? touchInput : keyboardInput;

        float currentSpeed = moveSpeed * airControlMultiplier;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        ClampToScreen();
    }

    void ClampToScreen()
    {
        if (mainCamera == null) return;

        float camHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float camX = mainCamera.transform.position.x;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, camX - camHalfWidth, camX + camHalfWidth);
        transform.position = pos;
    }

    public void SetTouchInput(float value)
    {
        touchInput = value;
    }
}