using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
    [Header("Squash & Stretch Ayarları")]
    public float squashScaleY = 0.7f;
    public float returnSpeed = 12f;

    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
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
}