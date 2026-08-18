using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ComboManager.Instance != null)
            {
                ComboManager.Instance.AddCombo();

                int comboCount = ComboManager.Instance.GetComboCount();
                for (int i = 0; i < comboCount; i++)
                {
                    CoinManager.Instance.AddCoin();
                    ScoreManager.Instance.AddCoin();
                }
            }
            else
            {
                CoinManager.Instance.AddCoin();
                ScoreManager.Instance.AddCoin();
            }

            Destroy(gameObject);
        }
    }
}