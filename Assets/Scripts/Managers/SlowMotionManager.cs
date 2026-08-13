using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    public static SlowMotionManager Instance;

    [Header("Slow Motion Ayarları")]
    public float duration = 10f;
    
    // 1 = normal hız
    // 0.25 = normalin %25'i
    public float slowFactor = 0.25f;

    private float timer = 0f;

    public bool IsActive => timer > 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = 0f;
            }
        }
    }

    public void Activate()
    {
        timer = duration;
    }

    public float GetSpeed(float normalSpeed)
    {
        if (!IsActive)
            return normalSpeed;

        return normalSpeed * slowFactor;
    }

    public float GetDelay(float normalDelay)
    {
        if (!IsActive)
            return normalDelay;

        return normalDelay / slowFactor;
    }

    public float GetRemainingTime()
    {
        return timer;
    }
}