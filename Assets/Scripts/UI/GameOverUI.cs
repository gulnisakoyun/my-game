using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;

    void OnEnable()
    {
        if (ScoreManager.Instance == null) return;

        int finalScore;
        int highScore;
        ScoreManager.Instance.CalculateFinalScore(out finalScore, out highScore);

        if (finalScoreText != null)
        {
            finalScoreText.text = "SKOR: " + finalScore;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "REKOR: " + highScore;
        }
    }
}