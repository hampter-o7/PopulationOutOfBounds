using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{

    public void LoadGame()
    {
        Tutorial.isTutorial = false;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadTutorial()
    {
        Tutorial.isTutorial = true;
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

    private void Start()
    {
        StartCoroutine(SetAllButtonClicks());
    }

    private IEnumerator SetAllButtonClicks()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (Button button in Resources.FindObjectsOfTypeAll<Button>()) button.onClick.AddListener(() => FindFirstObjectByType<SoundManager>().PlaySFX(1));
    }
}
