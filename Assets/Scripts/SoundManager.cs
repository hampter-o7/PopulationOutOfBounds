using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("-------Audio Source-------")]
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sFX;

    [Header("-------Audio Clip-------")]
    [SerializeField] AudioClip MainThemeMusic;
    [SerializeField] AudioClip BearThemeMusic;

    public static SoundManager instance;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private float audioTransitionTime = 2000f;



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
        music.clip = MainThemeMusic;
        music.Play();

        audioSources.Add(music);
        audioSources.Add(sFX);
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
