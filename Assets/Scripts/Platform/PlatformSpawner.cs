using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;
    [Range(0f, 1f)] public float movingPlatformChance = 0.25f;
    public GameObject breakingPlatformPrefab;
    public float difficultyStartHeight = 20f;
    public float difficultyMaxHeight = 100f;
    public GameObject mysteryPlatformPrefab;
    [Range(0f, 1f)] public float mysteryPlatformChance = 0.06f;
    public GameObject coinPrefab;
    [Range(0f, 1f)] public float coinSpawnChance = 0.45f;
    public float coinYOffset = 0.6f;
    public int platformCount = 10;
    public float minY = 1.5f;
    public float maxY = 3f;
    public float xRange = 4f;
    public GameObject magnetPrefab;
    [Range(0f, 1f)] public float magnetSpawnChance = 0.08f;
    public GameObject rocketPrefab;
    [Range(0f, 1f)] public float rocketSpawnChance = 0.02f;
    public float movingPlatformMoveDistance = 1f;

    public Transform player;
    public float spawnDistanceAhead = 7f;
    public float destroyDistanceBelow = 10f;

    private float highestY = 0f;
    private float lastPlatformX = 0f;
    private List<GameObject> spawnedPlatforms = new List<GameObject>();

    float GetDifficultyFactor(float currentHeight)
    {
        float t = (currentHeight - difficultyStartHeight) / (difficultyMaxHeight - difficultyStartHeight);
        return Mathf.Clamp01(t);
    }

    void SpawnPickup(Transform parentPlatform, float y)
    {
        float pickupRoll = Random.value;
        float rocketThreshold = rocketSpawnChance;
        float magnetThreshold = rocketThreshold + magnetSpawnChance;
        float coinThreshold = magnetThreshold + coinSpawnChance;

        Vector3 pos = new Vector3(parentPlatform.position.x, y + coinYOffset, 0f);
        GameObject spawnedPickup = null;

        if (pickupRoll <= rocketThreshold)
        {
            spawnedPickup = Instantiate(rocketPrefab, pos, Quaternion.identity);
        }
        else if (pickupRoll <= magnetThreshold)
        {
            spawnedPickup = Instantiate(magnetPrefab, pos, Quaternion.identity);
        }
        else if (pickupRoll <= coinThreshold)
        {
            spawnedPickup = Instantiate(coinPrefab, pos, Quaternion.identity);
        }

        if (spawnedPickup != null)
        {
            PickupFollower follower = spawnedPickup.AddComponent<PickupFollower>();
            follower.target = parentPlatform;
        }
    }

    void Start()
    {
        float currentY = 0f;

        for (int i = 0; i < platformCount; i++)
        {
            currentY += Random.Range(minY, maxY);

            float difficulty = GetDifficultyFactor(currentY);
            float currentMovingChance = Mathf.Lerp(movingPlatformChance, 0.5f, difficulty);
            float currentBreakingChance = Mathf.Lerp(0f, 0.25f, difficulty);

            float roll = Random.value;
            bool isMystery = roll <= mysteryPlatformChance;
            bool isBreaking = !isMystery && roll <= (mysteryPlatformChance + currentBreakingChance);
            bool isMoving = !isMystery && !isBreaking && roll <= (mysteryPlatformChance + currentBreakingChance + currentMovingChance);

            float effectiveXRange = isMoving ? Mathf.Max(xRange - movingPlatformMoveDistance, 1.5f) : xRange;

            float randomX;
            int attempts = 0;
            do
            {
                randomX = Random.Range(-effectiveXRange, effectiveXRange);
                attempts++;
            } while (Mathf.Abs(randomX - lastPlatformX) < 2.15f && attempts < 20);

            lastPlatformX = randomX;

            Vector3 spawnPosition = new Vector3(randomX, currentY, 0f);

            GameObject prefabToSpawn = isMystery ? mysteryPlatformPrefab : (isBreaking ? breakingPlatformPrefab : (isMoving ? movingPlatformPrefab : platformPrefab));
            GameObject newPlatform = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            if (isMoving)
            {
                MovingPlatform mp = newPlatform.GetComponent<MovingPlatform>();
                if (mp != null) mp.moveDistance = movingPlatformMoveDistance;
            }

            spawnedPlatforms.Add(newPlatform);

            if (!isMystery)
            {
                SpawnPickup(newPlatform.transform, currentY);
            }
        }

        highestY = currentY;
    }

    void Update()
    {
        if (player != null && player.position.y + spawnDistanceAhead > highestY)
        {
            SpawnPlatform();
        }

        if (player != null)
        {
            for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
            {
                if (spawnedPlatforms[i] == null) continue;

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
        highestY += Random.Range(minY, maxY);

        float difficulty = GetDifficultyFactor(highestY);
        float currentMovingChance = Mathf.Lerp(movingPlatformChance, 0.5f, difficulty);
        float currentBreakingChance = Mathf.Lerp(0f, 0.25f, difficulty);

        float roll = Random.value;
        bool isMystery = roll <= mysteryPlatformChance;
        bool isBreaking = !isMystery && roll <= (mysteryPlatformChance + currentBreakingChance);
        bool isMoving = !isMystery && !isBreaking && roll <= (mysteryPlatformChance + currentBreakingChance + currentMovingChance);

        float effectiveXRange = isMoving ? Mathf.Max(xRange - movingPlatformMoveDistance, 1.5f) : xRange;

        float randomX;
        int attempts = 0;
        do
        {
            randomX = Random.Range(-effectiveXRange, effectiveXRange);
            attempts++;
        } while (Mathf.Abs(randomX - lastPlatformX) < 2.15f && attempts < 20);

        lastPlatformX = randomX;

        Vector3 spawnPosition = new Vector3(randomX, highestY, 0f);

        GameObject prefabToSpawn = isMystery ? mysteryPlatformPrefab : (isBreaking ? breakingPlatformPrefab : (isMoving ? movingPlatformPrefab : platformPrefab));
        GameObject newPlatform = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        if (isMoving)
        {
            MovingPlatform mp = newPlatform.GetComponent<MovingPlatform>();
            if (mp != null) mp.moveDistance = movingPlatformMoveDistance;
        }

        spawnedPlatforms.Add(newPlatform);

        if (!isMystery)
        {
            SpawnPickup(newPlatform.transform, highestY);
        }
    }
}

// Eşyaların yassılaşmadan platformu TAM MERKEZDEN takip etmesini sağlayan güncel kod
public class PickupFollower : MonoBehaviour
{
    public Transform target;
    private float yOffset; // Artık sadece Y eksenindeki boşluğu tutuyoruz

    void Start()
    {
        if (target != null)
        {
            // Sadece Y farkını al, X'i alma ki hep ortada kalsın
            yOffset = transform.position.y - target.position.y;
        }
    }

    void Update()
    {
        if (target != null)
        {
            // X ekseninde direkt platformun ortasına (target.position.x) kilitlen
            transform.position = new Vector3(
                target.position.x, 
                target.position.y + yOffset, 
                transform.position.z
            );
        }
        else
        {
            Destroy(gameObject);
        }
    }
}