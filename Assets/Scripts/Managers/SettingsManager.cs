using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("UI Referanslari")]
    public Toggle musicToggle;
    public Toggle sfxToggle;
    public Toggle vibrationToggle;

    public bool MusicOn { get; private set; } = true;
    public bool SfxOn { get; private set; } = true;
    public bool VibrationOn { get; private set; } = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    void Start()
    {
        BindToggles(musicToggle, sfxToggle, vibrationToggle);
    }

    public void BindToggles(Toggle music, Toggle sfx, Toggle vibration)
    {
        musicToggle = music;
        sfxToggle = sfx;
        vibrationToggle = vibration;

        if (musicToggle != null)
        {
            musicToggle.onValueChanged.RemoveAllListeners();
            musicToggle.isOn = MusicOn;
            musicToggle.onValueChanged.AddListener(SetMusic);
        }

        if (sfxToggle != null)
        {
            sfxToggle.onValueChanged.RemoveAllListeners();
            sfxToggle.isOn = SfxOn;
            sfxToggle.onValueChanged.AddListener(SetSfx);
        }

        if (vibrationToggle != null)
        {
            vibrationToggle.onValueChanged.RemoveAllListeners();
            vibrationToggle.isOn = VibrationOn;
            vibrationToggle.onValueChanged.AddListener(SetVibration);
        }
    }

    public void SetMusic(bool value)
    {
        MusicOn = value;
        PlayerPrefs.SetInt("MusicOn", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetSfx(bool value)
    {
        SfxOn = value;
        PlayerPrefs.SetInt("SfxOn", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetVibration(bool value)
    {
        VibrationOn = value;
        PlayerPrefs.SetInt("VibrationOn", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        MusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        SfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;
        VibrationOn = PlayerPrefs.GetInt("VibrationOn", 1) == 1;
    }
}
