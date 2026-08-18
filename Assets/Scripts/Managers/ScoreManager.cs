using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Elemanları")]
    public TextMeshProUGUI scoreText;
    
    [Header("Skor Ayarları")]
    public int coinBonusPuan = 50; // Oyun bitince her bir coin kaç ekstra puan versin?

    private int currentScore = 0;
    private int totalCoins = 0;

    void Awake()
    {
        Instance = this;
    }

    // Senin orijinal fonksiyonun: Platforma değdikçe skoru 1 artırır
    public void AddPoint()
    {
        currentScore++;
        UpdateScoreText();
    }

    // Senin orijinal fonksiyonun: UI metnini günceller
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    // Senin orijinal fonksiyonun: Skoru başka yerlere gönderir
    public int GetScore()
    {
        return currentScore;
    }

    // YENİ: Altınları saymak için (Anlık skoru etkilemez)
    public void AddCoin(int amount = 1)
    {
        totalCoins += amount;
    }

    // YENİ: Gül'ün Game Over ekranı açıldığında final skoru ve rekoru hesaplamak için
    public void CalculateFinalScore(out int finalScore, out int highScore)
    {
        // Kendi skorun ile coin bonusunu birleştir
        finalScore = currentScore + (totalCoins * coinBonusPuan);

        // Telefonun hafızasındaki eski rekoru çağır (yoksa 0 getir)
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Eğer yeni rekor kırdıysak, hafızadaki rekoru güncelle ve kaydet
        if (finalScore > highScore)
        {
            highScore = finalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save(); 
        }
    }
}