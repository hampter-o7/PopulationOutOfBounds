using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{

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
        if (soundManager.GetMusicName().Equals("PopulationOutOfBounds - Bear Theme")) soundManager.PlayMainTheme();
        SceneManager.LoadScene("WinCreditsScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
