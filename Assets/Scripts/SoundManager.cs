using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("-------Audio Sources-------")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource SFX;
    [Header("-------Audio Clips-------")]
    [SerializeField] private AudioClip[] musicThemes;
    [SerializeField] private AudioClip[] bearEating;
    [SerializeField] private AudioClip[] mouseClicks;
    [SerializeField] private AudioClip[] fartNoises;
    [SerializeField] private AudioClip[] fenceNoises;
    private AudioClip[][] SFXSounds;
    private static SoundManager instance;
    private readonly float fadeDuration = 0.5f;
    private float currentMusicVolume;
    private float currentSFXVolume;

    public void PlayMainTheme()
    {
        music.clip = musicThemes[0];
        music.Play();
    }

    public void PlayBearTheme()
    {
        music.clip = musicThemes[1];
        music.Play();
    }

    public void PlaySFX(int clipNum)
    {
        SFX.PlayOneShot(SFXSounds[clipNum][Random.Range(0, SFXSounds[clipNum].Length)]);
    }

    public float GetMusicVolume()
    {
        return currentMusicVolume;
    }

    public void ChangeMusicVolume(float value)
    {
        StartCoroutine(FadeAudio(music, value, fadeDuration));
        currentMusicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public float GetSFXVolume()
    {
        return currentSFXVolume;
    }

    public void ChangeSFXVolume(float value)
    {
        StartCoroutine(FadeAudio(SFX, value, fadeDuration));
        currentSFXVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public string GetMusicName()
    {
        return music.clip.name;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetupAudio();
    }

    private void SetupAudio()
    {
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            currentSFXVolume = 0.5f;
            currentMusicVolume = 0.5f;
        }
        else
        {
            currentSFXVolume = PlayerPrefs.GetFloat("SFXVolume");
            currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }
        music.volume = currentMusicVolume;
        SFX.volume = currentMusicVolume;
        music.loop = true;
        PlayMainTheme();
        SFXSounds = new AudioClip[][] { bearEating, mouseClicks, fartNoises, fenceNoises };
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
