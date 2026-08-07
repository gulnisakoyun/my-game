using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoin();
            Destroy(gameObject);
        }
    }
}
