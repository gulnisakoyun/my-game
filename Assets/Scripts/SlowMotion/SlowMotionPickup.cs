using UnityEngine;

public class SlowMotionPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER: " + other.gameObject.name + " tag=" + other.tag);
        TryActivate(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION: " + collision.gameObject.name + " tag=" + collision.gameObject.tag);
        TryActivate(collision.gameObject);
    }

    void TryActivate(GameObject obj)
    {
        if (!obj.CompareTag("Player"))
        {
            Debug.Log("Player değil, çıkıldı: " + obj.name);
            return;
        }

        Debug.Log("Player doğrulandı. Manager instance null mu? " + (SlowMotionManager.Instance == null));

        if (SlowMotionManager.Instance != null)
        {
            SlowMotionManager.Instance.Activate();
            Debug.Log("Activate() çağrıldı. IsActive şimdi: " + SlowMotionManager.Instance.IsActive);
        }

        Destroy(gameObject);
    }
}