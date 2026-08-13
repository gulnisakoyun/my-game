using UnityEngine;

public class SlowMotionPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (SlowMotionManager.Instance == null)
        {
            Debug.LogError("SlowMotionManager bulunamadi!");
            return;
        }

        SlowMotionManager.Instance.Activate();

        Debug.Log("SLOW MOTION AKTIF! 10 saniye.");

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (SlowMotionManager.Instance == null)
        {
            Debug.LogError("SlowMotionManager bulunamadi!");
            return;
        }

        SlowMotionManager.Instance.Activate();

        Debug.Log("SLOW MOTION AKTIF! 10 saniye.");

        Destroy(gameObject);
    }
}