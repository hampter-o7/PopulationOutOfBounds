using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("-------Audio Source-------")]
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sFX;

    [Header("-------Audio Clip-------")]
    [SerializeField] AudioClip MainThememusic;
    [SerializeField] AudioClip BearThememusic;

    public static SoundManager instance;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private float audioTransitionTime = 2000f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
        music.clip = MainThememusic;
        music.Play();

        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSources.Add(audioSource);
        }
    }

    public void PlayMainTheme()
    {
        music.clip = MainThememusic;
        music.Play();
    }

    public void PlayBearTheme()
    {
        music.clip = BearThememusic;
        music.Play();
    }


    public void MuteSound()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = Mathf.Lerp(1f, 0f, audioTransitionTime);
            Debug.Log("Set audio of " + audioSource.name + " to 0");
        }
    }

    public void EnableSound()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, audioTransitionTime);
            Debug.Log("Set audio of " + audioSource.name + " to 1");
        }
    }
}
