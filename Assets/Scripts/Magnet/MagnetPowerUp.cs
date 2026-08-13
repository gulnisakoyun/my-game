using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    public float magnetDuration = 6f; // magnet kac saniye aktif kalsin

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MagnetEffect.Instance.ActivateMagnet(magnetDuration);

            PlayerFeedback feedback = other.GetComponent<PlayerFeedback>();
            if (feedback != null)
            {
                feedback.PlayBounce();
            }

            FloatingText.Create("Magnet!", transform.position, Color.magenta);

            Destroy(gameObject);
        }
    }
}