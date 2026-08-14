using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    public static SlowMotionManager Instance;

    public float slowMotionDuration = 10f;
    [Range(0.05f, 1f)]
    public float slowFactor = 0.25f;

    public UnityEngine.UI.Image slowMotionIndicator; // YENİ
    public float blinkSpeed = 5f;                    // YENİ

    public bool IsActive { get; private set; }
    public float CurrentFactor => IsActive ? slowFactor : 1f;

    private float timer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!IsActive)
        {
            if (slowMotionIndicator != null && slowMotionIndicator.gameObject.activeSelf)
                slowMotionIndicator.gameObject.SetActive(false);
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            IsActive = false;
            return;
        }

        // YENİ: Magnet ile aynı blink mantığı
        if (slowMotionIndicator != null)
        {
            if (!slowMotionIndicator.gameObject.activeSelf)
                slowMotionIndicator.gameObject.SetActive(true);

            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            Color c = slowMotionIndicator.color;
            c.a = alpha;
            slowMotionIndicator.color = c;
        }
    }

    public void Activate()
    {
        IsActive = true;
        timer = slowMotionDuration;
    }
}