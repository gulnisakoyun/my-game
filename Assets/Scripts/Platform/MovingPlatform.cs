using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveSpeed = 2f;

    private Vector3 startPosition;
    private Transform playerOnPlatform;
    private Vector3 lastPosition;
    private float movementTimer = 0f;

    void Start()
    {
        startPosition = transform.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        float slowFactor = (SlowMotionManager.Instance != null) ? SlowMotionManager.Instance.CurrentFactor : 1f;

        movementTimer += Time.deltaTime * moveSpeed * slowFactor;
        float offset = Mathf.PingPong(movementTimer, moveDistance * 2) - moveDistance;
        transform.position = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z);

        if (playerOnPlatform != null)
        {
            Vector3 delta = transform.position - lastPosition;
            playerOnPlatform.position += delta;
        }

        lastPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.transform.position.y < transform.position.y) return;

        playerOnPlatform = collision.transform;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = null;
        }
    }
}