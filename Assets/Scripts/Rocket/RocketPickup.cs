using UnityEngine;

public class RocketPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerJump pj = other.GetComponent<PlayerJump>();
            if (pj != null)
            {
                pj.GrantRocket();
            }
            Destroy(gameObject);
        }
    }
}