using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI coinsThisRunText;
    public TextMeshProUGUI totalCoinsText;

    void OnEnable()
    {
        if (ScoreManager.Instance == null) return;

        int finalScore, highScore, coinsThisRun, totalCoinsAllTime;
        ScoreManager.Instance.CalculateFinalScore(out finalScore, out highScore, out coinsThisRun, out totalCoinsAllTime);

        if (finalScoreText != null) finalScoreText.text = "SKOR: " + finalScore;
        if (highScoreText != null) highScoreText.text = "REKOR: " + highScore;
        if (coinsThisRunText != null) coinsThisRunText.text = "COIN: " + coinsThisRun;
        if (totalCoinsText != null) totalCoinsText.text = "TOPLAM COIN: " + totalCoinsAllTime;
    }
}