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

    AudioClip[][] SFXSounds = new AudioClip[3][];

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
        music.loop = true;
        music.clip = MainThemeMusic;
        music.Play();
        bearEating = new AudioClip[] { bearEating1 };
        mouseClick = new AudioClip[] { mouseClick1, mouseClick2 };
        fartNoise = new AudioClip[] { fartNoise1, fartNoise2 };
        SFXSounds[0] = bearEating;
        SFXSounds[1] = mouseClick;
        SFXSounds[2] = fartNoise;
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
    }

    public void ChangeSFXVolume(float value)
    {
        StartCoroutine(FadeAudio(SFX, value, fadeDuration));
        currentSFXVolume = value;
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
