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
    public bool isSFXMuted = false;
    public bool isMusicMuted = false;


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

    public void MuteEnableMusic()
    {
        isMusicMuted = !isMusicMuted;
        music.volume = isMusicMuted ? 0 : 1;
    }
    public void MuteEnableSFX()
    {
        isSFXMuted = !isSFXMuted;
        sFX.volume = isSFXMuted ? 0 : 1;
    }
}
