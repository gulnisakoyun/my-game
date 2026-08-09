using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{

    public GameObject platformPrefab;
    public GameObject coinPrefab;
    [Range(0f, 1f)]
    public float coinSpawnChance = 0.45f; // %45 ihtimalle coin cikar
    public float coinYOffset = 0.6f; // platformun ne kadar ustune konsun
    public int platformCount = 10;
    public float minY = 1.5f;
    public float maxY = 3f;
    public float xRange = 4f;
    public GameObject magnetPrefab;
    [Range(0f, 1f)]
    public float magnetSpawnChance = 0.08f; // %8 ihtimalle magnet cikar


    public Transform player; //oyuncunun pozisyon takibi
    public float spawnDistanceAhead = 7f; // oyuncunun kac birim ustune kadar platform hazır olsun
    public float destroyDistanceBelow  = 10f; // oyuncunun kac birim altındakiler silinsin

    private float highestY = 0f;
    private float lastPlatformX = 0f;
    private List<GameObject> spawnedPlatforms = new List<GameObject>();
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
{
    float currentY = 0f;

    for (int i = 0; i < platformCount; i++)
    {
        float randomX;
        do
        {
            randomX = Random.Range(-xRange, xRange);
        } while (Mathf.Abs(randomX - lastPlatformX) < 2.15f);

        lastPlatformX = randomX;

        currentY += Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(randomX, currentY, 0f);
        GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        spawnedPlatforms.Add(newPlatform);

        if (Random.value <= magnetSpawnChance)
        {
            Vector3 magnetPosition = new Vector3(randomX, spawnPosition.y + coinYOffset, 0f);
            Instantiate(magnetPrefab, magnetPosition, Quaternion.identity);
        }
        else if (Random.value <= coinSpawnChance)
        {
            Vector3 coinPosition = new Vector3(randomX, spawnPosition.y + coinYOffset, 0f);
            Instantiate(coinPrefab, coinPosition, Quaternion.identity);
        }
    }

    highestY = currentY;
}

    // Update is called once per frame
    void Update()
{
    // Oyuncu, en yüksek platforma yaklaştıysa yeni platform ekle
    if (player != null && player.position.y + spawnDistanceAhead > highestY)
    {
        SpawnPlatform();
    }

    // Oyuncunun çok altında kalan platformları sil
    if (player != null)
    {
        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            if (spawnedPlatforms[i] == null) continue; // zaten silinmişse atla

            if (spawnedPlatforms[i].transform.position.y < player.position.y - destroyDistanceBelow)
            {
                Destroy(spawnedPlatforms[i]);
                spawnedPlatforms.RemoveAt(i);
            }
        }
    }
}
void SpawnPlatform()
{
    float randomX;
    do
    {
        randomX = Random.Range(-xRange, xRange);
    } while (Mathf.Abs(randomX - lastPlatformX) < 2.15f);

    lastPlatformX = randomX;
    highestY += Random.Range(minY, maxY);

    Vector3 spawnPosition = new Vector3(randomX, highestY, 0f);
    GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

    spawnedPlatforms.Add(newPlatform);

    if (Random.value <= magnetSpawnChance)
    {
        Vector3 magnetPosition = new Vector3(randomX, spawnPosition.y + coinYOffset, 0f);
        Instantiate(magnetPrefab, magnetPosition, Quaternion.identity);
    }
    else if (Random.value <= coinSpawnChance)
    {
        Vector3 coinPosition = new Vector3(randomX, spawnPosition.y + coinYOffset, 0f);
        Instantiate(coinPrefab, coinPosition, Quaternion.identity);
    }
}
}
