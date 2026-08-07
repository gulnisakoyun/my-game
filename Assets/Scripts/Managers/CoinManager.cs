using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public TMPro.TextMeshProUGUI coinText; 
    private int coinCount = 0;

    void Awake()
    {
        Instance = this;
    }

    public void AddCoin()
    {
        coinCount++;
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinCount;
        }
    }

    public int GetCoinCount()
    {
        return coinCount;
    }
}
