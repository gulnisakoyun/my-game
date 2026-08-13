using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
    [Header("Squash & Stretch Ayarları")]
    public float squashScaleY = 0.7f;
    public float returnSpeed = 12f;

    [Header("Rocket & Double Jump Renkleri")]
    public Color rocketColor = Color.red;
    public Color doubleJumpColor = Color.cyan;

    private Vector3 originalScale;
    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        originalScale = transform.localScale;
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    void Update()
    {
        // Her karede, şu anki boyuttan orijinal boyuta doğru yumuşakça yaklaş
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, returnSpeed * Time.deltaTime);
    }

    public void PlayBounce()
    {
        // Anlık olarak yassılt, Update() zaten onu normale geri toparlayacak
        transform.localScale = new Vector3(originalScale.x * 1.2f, originalScale.y * squashScaleY, originalScale.z);
    }

    public void PlayDoubleJump()
    {
        // Yatayda hafif sıkışma - dikeyde uzama (fırlama hissi)
        transform.localScale = new Vector3(originalScale.x * 0.8f, originalScale.y * 1.25f, originalScale.z);

        if (sr != null) StartCoroutine(FlashColor(doubleJumpColor));
    }

    public void PlayRocketStart()
    {
        if (sr != null) sr.color = rocketColor;
        transform.localScale = new Vector3(originalScale.x * 0.85f, originalScale.y * 1.3f, originalScale.z);
    }

    public void PlayRocketEnd()
    {
        if (sr != null) sr.color = originalColor;
    }

    private System.Collections.IEnumerator FlashColor(Color flashColor)
    {
        if (sr == null) yield break;
        sr.color = flashColor;
        yield return new WaitForSeconds(0.15f);
        sr.color = originalColor;
    }
}