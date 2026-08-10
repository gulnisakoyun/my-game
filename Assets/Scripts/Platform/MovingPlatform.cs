using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 2f; // ne kadar sağa-sola gidecek
    public float moveSpeed = 2f; // hareket hızı

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2) - moveDistance;
        transform.position = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z);
    }
}