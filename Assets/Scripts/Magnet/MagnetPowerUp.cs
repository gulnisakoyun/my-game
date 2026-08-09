using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    public float magnetDuration = 6f; // magnet kac saniye aktif kalsin

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MagnetEffect.Instance.ActivateMagnet(magnetDuration);
            Destroy(gameObject); // magnet toplanınca kaybolsun
        }
    }
}