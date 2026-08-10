using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 1, -10);

    private float highestY;

    void Start()
    {
        highestY = transform.position.y;
    }

    void LateUpdate()
    {
        if (player == null) return;

        float targetY = player.position.y + offset.y;

        // Kamera sadece daha yükseğe çıkarsa hareket eder, asla aşağı inmez
        if (targetY > highestY)
        {
            highestY = targetY;
        }

        Vector3 targetPosition = new Vector3(transform.position.x, highestY, offset.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}