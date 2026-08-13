using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Prefabları")]
    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;
    public GameObject breakingPlatformPrefab;
    public GameObject mysteryPlatformPrefab;

    [Header("Platform İhtimalleri")]
    [Range(0f, 1f)]
    public float movingPlatformChance = 0.25f;

    [Range(0f, 1f)]
    public float mysteryPlatformChance = 0.06f;

    [Header("Zorluk")]
    public float difficultyStartHeight = 20f;
    public float difficultyMaxHeight = 100f;

    [Header("Coin")]
    public GameObject coinPrefab;

    [Range(0f, 1f)]
    public float coinSpawnChance = 0.45f;

    public float coinYOffset = 0.6f;

    [Header("Magnet")]
    public GameObject magnetPrefab;

    [Range(0f, 1f)]
    public float magnetSpawnChance = 0.075f;

    [Header("Rocket")]
    public GameObject rocketPrefab;

    [Range(0f, 1f)]
    public float rocketSpawnChance = 0.05f;

    [Header("Slow Motion - Şimdilik Kapalı")]
    public GameObject slowMotionPrefab;

    [Range(0f, 1f)]
    public float slowMotionSpawnChance = 0f;

    [Header("Platform Ayarları")]
    public int platformCount = 10;
    public float minY = 4f;
    public float maxY = 5f;
    public float xRange = 3.925f;

    public float movingPlatformMoveDistance = 2f;

    [Header("Oyuncu")]
    public Transform player;

    public float spawnDistanceAhead = 7f;
    public float destroyDistanceBelow = 10f;

    private float highestY = 0f;
    private float lastPlatformX = 0f;

    private List<GameObject> spawnedPlatforms =
        new List<GameObject>();


    float GetDifficultyFactor(float currentHeight)
    {
        float t =
            (currentHeight - difficultyStartHeight) /
            (difficultyMaxHeight - difficultyStartHeight);

        return Mathf.Clamp01(t);
    }


    // PLATFORM ÜZERİNE COIN / MAGNET / ROCKET OLUŞTURUR
    void SpawnPickup(float randomX, float y)
    {
        float roll = Random.value;

        Vector3 pos = new Vector3(
            randomX,
            y + coinYOffset,
            0f
        );


        // ROCKET
        if (roll < rocketSpawnChance)
        {
            if (rocketPrefab != null)
            {
                Instantiate(
                    rocketPrefab,
                    pos,
                    Quaternion.identity
                );
            }

            return;
        }


        // MAGNET
        float magnetThreshold =
            rocketSpawnChance + magnetSpawnChance;

        if (roll < magnetThreshold)
        {
            if (magnetPrefab != null)
            {
                Instantiate(
                    magnetPrefab,
                    pos,
                    Quaternion.identity
                );
            }

            return;
        }


        // COIN
        float coinThreshold =
            magnetThreshold + coinSpawnChance;

        if (roll < coinThreshold)
        {
            if (coinPrefab != null)
            {
                Instantiate(
                    coinPrefab,
                    pos,
                    Quaternion.identity
                );
            }

            return;
        }

        // Slow Motion şimdilik burada KULLANILMIYOR.
    }


    void Start()
    {
        float currentY = 0f;

        for (int i = 0; i < platformCount; i++)
        {
            currentY += Random.Range(minY, maxY);

            SpawnNewPlatform(currentY);
        }

        highestY = currentY;
    }


    void Update()
    {
        // Oyuncunun önünde yeterli platform yoksa yeni platform oluştur
        if (player != null &&
            player.position.y + spawnDistanceAhead > highestY)
        {
            SpawnPlatform();
        }


        // Oyuncunun çok aşağısında kalan platformları sil
        if (player != null)
        {
            for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
            {
                if (spawnedPlatforms[i] == null)
                {
                    spawnedPlatforms.RemoveAt(i);
                    continue;
                }

                if (
                    spawnedPlatforms[i].transform.position.y
                    < player.position.y - destroyDistanceBelow
                )
                {
                    Destroy(spawnedPlatforms[i]);
                    spawnedPlatforms.RemoveAt(i);
                }
            }
        }
    }


    void SpawnPlatform()
    {
        highestY += Random.Range(minY, maxY);

        SpawnNewPlatform(highestY);
    }


    void SpawnNewPlatform(float y)
    {
        float difficulty = GetDifficultyFactor(y);


        // Zorluk arttıkça moving platform ihtimali artar
        float currentMovingChance =
            Mathf.Lerp(
                movingPlatformChance,
                0.5f,
                difficulty
            );


        // Zorluk arttıkça breaking platform ihtimali artar
        float currentBreakingChance =
            Mathf.Lerp(
                0f,
                0.25f,
                difficulty
            );


        float roll = Random.value;


        bool isMystery =
            roll <= mysteryPlatformChance;


        bool isBreaking =
            !isMystery &&
            roll <=
            mysteryPlatformChance +
            currentBreakingChance;


        bool isMoving =
            !isMystery &&
            !isBreaking &&
            roll <=
            mysteryPlatformChance +
            currentBreakingChance +
            currentMovingChance;


        float effectiveXRange =
            isMoving
            ? Mathf.Max(
                xRange - movingPlatformMoveDistance,
                1.5f
            )
            : xRange;


        float randomX;

        int attempts = 0;


        do
        {
            randomX =
                Random.Range(
                    -effectiveXRange,
                    effectiveXRange
                );

            attempts++;

        }
        while (
            Mathf.Abs(randomX - lastPlatformX) < 2.15f &&
            attempts < 20
        );


        lastPlatformX = randomX;


        Vector3 spawnPosition =
            new Vector3(
                randomX,
                y,
                0f
            );


        GameObject prefabToSpawn;


        if (isMystery)
        {
            prefabToSpawn = mysteryPlatformPrefab;
        }
        else if (isBreaking)
        {
            prefabToSpawn = breakingPlatformPrefab;
        }
        else if (isMoving)
        {
            prefabToSpawn = movingPlatformPrefab;
        }
        else
        {
            prefabToSpawn = platformPrefab;
        }


        if (prefabToSpawn == null)
        {
            Debug.LogError(
                "PlatformSpawner: Platform prefab eksik!"
            );

            return;
        }


        GameObject newPlatform =
            Instantiate(
                prefabToSpawn,
                spawnPosition,
                Quaternion.identity
            );


        // Moving platform ayarı
        if (isMoving)
        {
            MovingPlatform mp =
                newPlatform.GetComponent<MovingPlatform>();

            if (mp != null)
            {
                mp.moveDistance =
                    movingPlatformMoveDistance;
            }
        }


        spawnedPlatforms.Add(newPlatform);


        // Mystery kendi ödül sistemini kullanır.
        // Diğer platformlar pickup alabilir.
        if (!isMystery)
        {
            SpawnPickup(
                randomX,
                y
            );
        }
    }
}