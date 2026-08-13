using UnityEngine;

public class MysteryPlatform : MonoBehaviour
{
    [Header("Odul Prefablari")]
    public GameObject coinPrefab;
    public GameObject magnetPrefab;

    [Header("Odul Ayarlari")]
    public float rewardYOffset = 0.6f;
    public int coinRewardAmount = 3;
    public float coinSpreadX = 0.5f;

    private bool triggered = false;
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.transform.position.y < transform.position.y) return; // alttan geldiyse yoksay

        triggered = true;
        GiveRandomReward();
        StartCoroutine(PulseAnimation());
    }

    void GiveRandomReward()
    {
        int rewardIndex = Random.Range(0, 2);

        if (rewardIndex == 0)
        {
            GiveCoins();
        }
        else
        {
            Vector3 magnetPosition = transform.position + new Vector3(0f, rewardYOffset, 0f);
            Instantiate(magnetPrefab, magnetPosition, Quaternion.identity);
        }
    }

    void GiveCoins()
    {
        for (int i = 0; i < coinRewardAmount; i++)
        {
            float xOffset = (i - (coinRewardAmount - 1) / 2f) * coinSpreadX;
            Vector3 coinPosition = transform.position + new Vector3(xOffset, rewardYOffset, 0f);
            Instantiate(coinPrefab, coinPosition, Quaternion.identity);
        }
    }

    System.Collections.IEnumerator PulseAnimation()
    {
        Vector3 bigScale = originalScale * 1.3f;
        float duration = 0.15f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, bigScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(bigScale, originalScale, elapsed / duration);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}