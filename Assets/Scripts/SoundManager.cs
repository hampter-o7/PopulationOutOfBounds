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

    AudioClip[] bearEating;
    [SerializeField] AudioClip bearEating1;
    AudioClip[] mouseClick;
    [SerializeField] AudioClip mouseClick1;
    [SerializeField] AudioClip mouseClick2;

    AudioClip[] fartNoise;
    [SerializeField] AudioClip fartNoise1;
    [SerializeField] AudioClip fartNoise2;

    AudioClip[] fenceNoise;
    [SerializeField] AudioClip fenceNoise1;

    AudioClip[][] SFXSounds = new AudioClip[4][];

    public static SoundManager instance;
    private float fadeDuration = 0.5f;
    public float currentMusicVolume = 0.5f;
    public float currentSFXVolume = 0.5f;

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
    void Start()
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
        Debug.Log("Set MusicVolume from playerPrefs to: " + currentMusicVolume);
        Debug.Log("Set SFXVolume from playerPrefs to: " + currentSFXVolume);
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
