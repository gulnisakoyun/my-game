using UnityEngine;

public class PopInEffect : MonoBehaviour
{
    public float duration = 0.2f;

    private Vector3 targetScale;
    private float elapsed = 0f;

    void Awake()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / duration);
        }
    }
}
