using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("-------Audio Source-------")]
    public AudioSource music;
    public AudioSource SFX;

    [Header("-------Audio Clip-------")]
    [SerializeField] AudioClip MainThemeMusic;
    [SerializeField] AudioClip BearThemeMusic;

    [SerializeField] AudioClip bearEating;
    [SerializeField] AudioClip mouseClick;
    [SerializeField] AudioClip fartNoise;
    AudioClip[] SFXSounds = new AudioClip[3];

    public static SoundManager instance;
    private float fadeDuration = 0.5f;
    public float currentMusicVolume = 1;
    public float currentSFXVolume = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SFXSounds[0] = bearEating;
            SFXSounds[1] = mouseClick;
            SFXSounds[2] = fartNoise;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        currentSFXVolume = PlayerPrefs.GetFloat("SFXVolume");
        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        ChangeMusicVolume(currentMusicVolume);
        ChangeSFXVolume(currentSFXVolume);
        Debug.Log("Set MusicVolume from playerPrefs to: " + currentMusicVolume);
        Debug.Log("Set SFXVolume from playerPrefs to: " + currentSFXVolume);
        music.loop = true;
        music.clip = MainThemeMusic;
        music.Play();
    }

    public void PlayMainTheme()
    {
        music.clip = MainThemeMusic;
        music.Play();
    }

    public void PlayBearTheme()
    {
        music.clip = BearThemeMusic;
        music.Play();
    }

    public void PlaySFX(int clipNum)
    {
        SFX.PlayOneShot(SFXSounds[clipNum]);
    }

    public void ChangeMusicVolume(float value)
    {
        StartCoroutine(FadeAudio(music, value, fadeDuration));
        currentMusicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void ChangeSFXVolume(float value)
    {
        StartCoroutine(FadeAudio(SFX, value, fadeDuration));
        currentSFXVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private IEnumerator FadeAudio(AudioSource audioSource, float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
