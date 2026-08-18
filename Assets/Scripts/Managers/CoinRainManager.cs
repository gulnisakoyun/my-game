using UnityEngine;
using System.Collections;

public class CoinRainManager : MonoBehaviour
{
    [Header("Gerekli Referanslar")]
    public GameObject coinPrefab;
    public Transform player;
    
    [Header("Etkinlik Ayarları")]
    public int rainScoreInterval = 50; // Her kaç skorda bir yağmur başlasın?
    public float rainDuration = 5f; // Yağmur kaç saniye sürsün?
    public float spawnInterval = 0.15f; // Hangi sıklıkla altın çıksın?
    
    [Header("Yağmur Ayarları")]
    public float spawnHeightOffset = 8f; // Kameranın ne kadar üstünden yağsın?
    public float spawnWidth = 3.5f; // Sağa sola saçılma genişliği
    public float fallSpeed = 6f; // Altınların düşme hızı

    private int nextRainScore;
    private bool isRaining = false;

    void Start()
    {
        // Eğer player boşsa sahnede otomatik bul
        if (player == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null) player = pObj.transform;
        }
        
        // İlk hedef skoru belirle (Örn: 50)
        nextRainScore = rainScoreInterval;
    }

    void Update()
    {
        // ScoreManager sahnede var mı diye kontrol et
        if (ScoreManager.Instance != null)
        {
            int currentScore = ScoreManager.Instance.GetScore();

            // Eğer oyuncu hedef skora ulaştıysa (veya geçtiyse) ve o an zaten yağmur yağmıyorsa
            if (currentScore >= nextRainScore && !isRaining)
            {
                StartCoroutine(RainCoins());
                
                // Bir sonraki hedefi belirle (Örn: 50'ydi, şimdi 100 oldu)
                nextRainScore += rainScoreInterval; 
            }
        }
    }

    IEnumerator RainCoins()
    {
        isRaining = true; // Yağmur başladı bayrağını çek (üst üste tetiklenmemesi için)
        float elapsed = 0f;

        // Yağmur süresi boyunca altın üretmeye devam et
        while (elapsed < rainDuration)
        {
            if (player != null)
            {
                SpawnSingleCoin();
            }
            elapsed += spawnInterval;
            yield return new WaitForSeconds(spawnInterval);
        }
        
        isRaining = false; // Yağmur bitti
    }

    void SpawnSingleCoin()
    {
        // Oyuncunun hizasında, biraz sağda/solda ve tepede bir nokta belirle
        float randomX = player.position.x + Random.Range(-spawnWidth, spawnWidth);
        Vector3 spawnPos = new Vector3(randomX, player.position.y + spawnHeightOffset, 0f);
        
        // Altını yarat
        GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        
        // Altının aşağı düşmesi için küçük kod parçasını ekle
        FallingCoin fallingScript = coin.AddComponent<FallingCoin>();
        fallingScript.speed = fallSpeed;
    }
}

// Yağmur altınlarının aşağı düşmesini ve ekrandan çıkınca silinmesini sağlayan eklenti
public class FallingCoin : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Altını aşağı doğru hareket ettir
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Oyuncunun 15 birim aşağısına düştüyse sil (hafızayı şişirmemek için)
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null && transform.position.y < player.position.y - 15f)
        {
            Destroy(gameObject);
        }
    }
}