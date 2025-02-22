using System.Collections.Generic;
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
        SFX.clip = bearEating;
        SFX.Play();
    }

    public void MuteEnableMusic()
    {
        isMusicMuted = !isMusicMuted;
        music.volume = isMusicMuted ? 0 : 1;
    }
    public void MuteEnableSFX()
    {
        isSFXMuted = !isSFXMuted;
        SFX.volume = isSFXMuted ? 0 : 1;
    }
}
