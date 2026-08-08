using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
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
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public void SetTouchInput(float value)
    {
        touchInput = value;
    }
}
