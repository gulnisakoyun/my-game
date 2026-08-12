using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance;

    [Header("Combo Ayarları")]
    public TextMeshProUGUI comboText;
    public float comboWindow = 2f; // Bu süre içinde coin toplamazsan combo sıfırlanır

    private int comboCount = 0;
    private float comboTimer = 0f;

    void Awake()
    {
        Instance = this;
        UpdateComboText();
    }

    void Update()
    {
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

    public void AddCombo()
    {
        comboCount++;
        comboTimer = comboWindow;
        UpdateComboText();
    }

    public int GetComboCount()
    {
        return comboCount;
    }

    void ResetCombo()
    {
        comboCount = 0;
        UpdateComboText();
    }

    void UpdateComboText()
    {
        if (comboText == null) return;

        if (comboCount > 1)
        {
            comboText.text = "COMBO x" + comboCount;
            comboText.gameObject.SetActive(true);
        }
        else
        {
            comboText.gameObject.SetActive(false);
        }
    }
}