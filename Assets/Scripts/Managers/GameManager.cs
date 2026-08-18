using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Referanslar")]
    public Transform player;
    public Camera mainCamera;
    public GameObject gameOverPanel; // Gul'un Gun 6'da ekleyecegi panel, simdilik bos birakilabilir

    [Header("Ayarlar")]
    public float fallThreshold = 2f;

    public bool IsGameOver { get; private set; } = false;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        IsGameOver = false;
    }

    void Update()
    {
        if (IsGameOver) return;

        float cameraBottomEdge = mainCamera.transform.position.y - mainCamera.orthographicSize;

        if (player.position.y < cameraBottomEdge - fallThreshold)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Debug.Log("GAME OVER!");
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        IsGameOver = false;
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void GoToMainMenu()
    {
        IsGameOver = false;
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}