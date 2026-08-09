using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetEffect : MonoBehaviour
{
    public static MagnetEffect Instance;

    public float pullRadius = 4f;
    public float pullSpeed = 8f;
    public UnityEngine.UI.Image magnetIndicator;
    public float blinkSpeed = 5f;

    private bool isActive = false;
    private Transform player;
    private List<Transform> pulledCoins = new List<Transform>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ActivateMagnet(float duration)
    {
        StopCoroutine(nameof(MagnetTimer));
        StartCoroutine(MagnetTimer(duration));
    }

    IEnumerator MagnetTimer(float duration)
    {
        isActive = true;
        if (magnetIndicator != null) magnetIndicator.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        isActive = false;
        if (magnetIndicator != null) magnetIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isActive && magnetIndicator != null)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            Color c = magnetIndicator.color;
            c.a = alpha;
            magnetIndicator.color = c;
        }

        if (player == null) return;

        // Magnet aktifken yakindaki yeni coinleri listeye ekle
        if (isActive)
        {
            Collider2D[] coinsInRange = Physics2D.OverlapCircleAll(player.position, pullRadius);
            foreach (Collider2D col in coinsInRange)
            {
                if (col.CompareTag("Coin") && !pulledCoins.Contains(col.transform))
                {
                    pulledCoins.Add(col.transform);
                }
            }
        }

        // Listedeki tum coinleri cek (magnet bitse bile bu coinler gelmeye devam eder)
        for (int i = pulledCoins.Count - 1; i >= 0; i--)
        {
            if (pulledCoins[i] == null)
            {
                pulledCoins.RemoveAt(i);
                continue;
            }

            pulledCoins[i].position = Vector3.MoveTowards(
                pulledCoins[i].position,
                player.position,
                pullSpeed * Time.deltaTime
            );
        }
    }
}