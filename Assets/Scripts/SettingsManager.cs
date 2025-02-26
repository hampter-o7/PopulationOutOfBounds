using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("----------Sliders----------")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [Header("----------Objects----------")]
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject escMenu;
    private SoundManager soundManager;


    public void ShowSettings()
    {
        UpdateSettingsText();
        settings.SetActive(true);
        escMenu.SetActive(false);
    }

    public void UpdateSettingsText()
    {
        if (soundManager == null) soundManager = FindFirstObjectByType<SoundManager>();
        musicSlider.value = soundManager.GetMusicVolume();
        SFXSlider.value = soundManager.GetSFXVolume();
    }

    public void ChangeMusicVolume()
    {
        soundManager.ChangeMusicVolume(musicSlider.value);
        UpdateSettingsText();
    }

    public void ChangeSFXVolume()
    {
        soundManager.ChangeSFXVolume(SFXSlider.value);
        UpdateSettingsText();
    }
}
