using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("-------Audio Sources-------")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource SFX;
    [Header("-------Audio Clips-------")]
    [SerializeField] private AudioClip MainThemeMusic;
    [SerializeField] private AudioClip BearThemeMusic;
    [SerializeField] private AudioClip bearEating1;
    [SerializeField] private AudioClip mouseClick1;
    [SerializeField] private AudioClip mouseClick2;
    [SerializeField] private AudioClip fartNoise1;
    [SerializeField] private AudioClip fartNoise2;
    [SerializeField] private AudioClip fenceNoise1;
    private AudioClip[] bearEating;
    private AudioClip[] mouseClick;
    private AudioClip[] fartNoise;
    private AudioClip[] fenceNoise;
    private readonly AudioClip[][] SFXSounds = new AudioClip[4][];
    private static SoundManager instance;
    private readonly float fadeDuration = 0.5f;
    private float currentMusicVolume = 0.5f;
    private float currentSFXVolume = 0.5f;

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
        SFX.PlayOneShot(SFXSounds[clipNum][Random.Range(0, SFXSounds[clipNum].Length)]);
    }

    public float getMusicVolume()
    {
        return currentMusicVolume;
    }

    public void ChangeMusicVolume(float value)
    {
        StartCoroutine(FadeAudio(music, value, fadeDuration));
        currentMusicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public float getSFXVolume()
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
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
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
        ChangeSFXVolume(currentSFXVolume);
        music.loop = true;
        music.clip = MainThemeMusic;
        music.Play();
        bearEating = new AudioClip[] { bearEating1 };
        mouseClick = new AudioClip[] { mouseClick1, mouseClick2 };
        fartNoise = new AudioClip[] { fartNoise1, fartNoise2 };
        fenceNoise = new AudioClip[] { fenceNoise1 };
        SFXSounds[0] = bearEating;
        SFXSounds[1] = mouseClick;
        SFXSounds[2] = fartNoise;
        SFXSounds[3] = fenceNoise;
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
