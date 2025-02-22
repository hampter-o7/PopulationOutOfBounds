using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainScene()
    {
        SoundManager soundManager = FindFirstObjectByType<SoundManager>();
        soundManager.PlayMainTheme();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCreditsScene()
    {
        SoundManager soundManager = FindFirstObjectByType<SoundManager>();
        Debug.Log(soundManager.music.clip.name);
        if (soundManager.music.clip.name.Equals("PopulationOutOfBounds - Bear Theme"))
        {
            soundManager.PlayMainTheme();
        }
        SceneManager.LoadScene("WinCreditsScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
