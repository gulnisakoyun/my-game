using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Elemanları")]
    public TextMeshProUGUI scoreText;

    private int currentScore = 0;
    private int totalCoins = 0;

    void Awake()
{
    Instance = this;
}

    public void AddPoint()
    {
        currentScore++;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void AddCoin(int amount = 1)
    {
        totalCoins += amount;
    }

    public int GetCoinsThisRun()
    {
        return totalCoins;
    }

    // Skor artik sadece platform sayisi - coin bonusu KALDIRILDI
    public void CalculateFinalScore(out int finalScore, out int highScore, out int coinsThisRun, out int totalCoinsAllTime)
    {
        finalScore = currentScore; // sadece platform sayisi
        coinsThisRun = totalCoins;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (finalScore > highScore)
        {
            highScore = finalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        totalCoinsAllTime = PlayerPrefs.GetInt("TotalCoins", 0) + totalCoins;
        PlayerPrefs.SetInt("TotalCoins", totalCoinsAllTime);

        PlayerPrefs.Save();
    }
}