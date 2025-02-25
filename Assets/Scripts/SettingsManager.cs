using TMPro;
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

    public void ShowSettings()
    {
        UpdateSettingsText();
        settings.SetActive(true);
        escMenu.SetActive(false);
    }

    public void UpdateSettingsText()
    {
        SoundManager soundManager = FindFirstObjectByType<SoundManager>();
        musicSlider.value = soundManager.getMusicVolume();
        SFXSlider.value = soundManager.getSFXVolume();
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

    public void PlayButtonClick()
    {
        FindFirstObjectByType<SoundManager>().GetComponent<SoundManager>().PlaySFX(1);
    }
}
