using TMPro;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public TextMeshProUGUI musicButton;
    public TextMeshProUGUI SFXButton;
    public GameObject settings;
    public GameObject escMenu;

    public void ShowSettings()
    {
        UpdateSettingsText();
        settings.SetActive(true);
        escMenu.SetActive(false);
    }

    public void UpdateSettingsText()
    {
        SoundManager soundManager = FindFirstObjectByType<SoundManager>();
        bool muteSFX = soundManager.isSFXMuted;
        musicButton.text = soundManager.isMusicMuted ? "MUSIC   OFF" : "MUSIC   ON";
        SFXButton.text = soundManager.isSFXMuted ? "SFX     OFF" : "SFX     ON";
    }

    public void MuteEnableMusic()
    {
        FindFirstObjectByType<SoundManager>().GetComponent<SoundManager>().MuteEnableMusic();
        UpdateSettingsText();
    }

    public void MuteEnableSFX()
    {
        FindFirstObjectByType<SoundManager>().GetComponent<SoundManager>().MuteEnableSFX();
        UpdateSettingsText();
    }
}
