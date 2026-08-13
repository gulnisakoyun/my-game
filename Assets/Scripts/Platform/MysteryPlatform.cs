using UnityEngine;

public class MysteryPlatform : MonoBehaviour
{
    [Header("Odul Prefablari")]
    public GameObject coinPrefab;
    public GameObject magnetPrefab;
    public GameObject rocketPrefab;
    public GameObject slowMotionPrefab;

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

        // Platform scriptinde collision normalinin yönü ters olduğu
        // için üstten gelmeyi -0.5 ile kontrol ediyoruz.
        bool landedOnTop = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                landedOnTop = true;
                break;
            }
        }

        if (!landedOnTop) return;

        triggered = true;

        GiveRandomReward();
        StartCoroutine(PulseAnimation());
    }

    void GiveRandomReward()
    {
        int rewardIndex = Random.Range(0, 4);

        switch (rewardIndex)
        {
            case 0:
                GiveCoins();
                break;

            case 1:
                if (magnetPrefab != null)
                {
                    Instantiate(
                        magnetPrefab,
                        GetRewardPosition(),
                        Quaternion.identity
                    );
                }
                break;

            case 2:
                if (rocketPrefab != null)
                {
                    Instantiate(
                        rocketPrefab,
                        GetRewardPosition(),
                        Quaternion.identity
                    );
                }
                break;

            case 3:
                if (slowMotionPrefab != null)
                {
                    Instantiate(
                        slowMotionPrefab,
                        GetRewardPosition(),
                        Quaternion.identity
                    );
                }
                break;
        }
    }

    Vector3 GetRewardPosition()
    {
        return transform.position +
               new Vector3(0f, rewardYOffset, 0f);
    }

    void GiveCoins()
    {
        if (coinPrefab == null) return;

        for (int i = 0; i < coinRewardAmount; i++)
        {
            float xOffset =
                (i - (coinRewardAmount - 1) / 2f)
                * coinSpreadX;

            Vector3 coinPosition =
                transform.position +
                new Vector3(xOffset, rewardYOffset, 0f);

            Instantiate(
                coinPrefab,
                coinPosition,
                Quaternion.identity
            );
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

            transform.localScale =
                Vector3.Lerp(
                    originalScale,
                    bigScale,
                    elapsed / duration
                );

            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            transform.localScale =
                Vector3.Lerp(
                    bigScale,
                    originalScale,
                    elapsed / duration
                );

            yield return null;
        }

        transform.localScale = originalScale;
    }
}