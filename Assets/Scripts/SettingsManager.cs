using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider SFXSlider;
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
        musicSlider.value = soundManager.currentMusicVolume;
        SFXSlider.value = soundManager.currentSFXVolume;
    }

    public void ChangeMusicVolume()
    {
        FindFirstObjectByType<SoundManager>().GetComponent<SoundManager>().ChangeMusicVolume(musicSlider.value);
        UpdateSettingsText();
    }

    public void ChangeSFXVolume()
    {
        FindFirstObjectByType<SoundManager>().GetComponent<SoundManager>().ChangeSFXVolume(SFXSlider.value);
        UpdateSettingsText();
    }
}
