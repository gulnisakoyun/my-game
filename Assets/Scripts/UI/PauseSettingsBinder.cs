using UnityEngine;
using UnityEngine.UI;

public class PauseSettingsBinder : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle sfxToggle;
    public Toggle vibrationToggle;

    void OnEnable()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.BindToggles(musicToggle, sfxToggle, vibrationToggle);
        }
    }
}
