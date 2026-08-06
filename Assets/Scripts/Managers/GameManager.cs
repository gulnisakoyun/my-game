using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Referanslar")]
    public Transform player;
    public Camera mainCamera;

    [Header("Ayarlar")]
    public float fallThreshold = 2f; // Kameranın altına ne kadar inince Game Over olsun

    private bool isGameOver = false;

    void Awake()
    {
        // Bu GameManager'dan sahnede sadece bir tane olmasını garantiler
        Instance = this;
    }

    void Update()
    {
        if (isGameOver) return;

        // Kameranın alt sınırını hesapla
        float cameraBottomEdge = mainCamera.transform.position.y - mainCamera.orthographicSize;

        // Oyuncu bu sınırın altına indiyse Game Over
        if (player.position.y < cameraBottomEdge - fallThreshold)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("GAME OVER!");
        Time.timeScale = 0f; // Oyunu tamamen durdurur (her şey donar)
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Zamanı normale döndür
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
