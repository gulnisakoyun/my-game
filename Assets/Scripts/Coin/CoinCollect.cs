using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoin();

            if (ComboManager.Instance != null)
            {
                ComboManager.Instance.AddCombo();

                if (scoreManager != null)
                {
                    int comboCount = ComboManager.Instance.GetComboCount();
                    for (int i = 0; i < comboCount; i++)
                    {
                        scoreManager.AddPoint();
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}