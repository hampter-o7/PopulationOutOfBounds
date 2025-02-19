using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI _animalCountText;
    [SerializeField] GameObject _canvas;
    [SerializeField] private int maxAnimalCount = 20;

    public int animalCount { get; private set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _animalCountText.text = "Current animal count: " + animalCount;
        CheckGameConditions();
    }

    public void increaseAnimalCount()
    {
        animalCount++;
        Debug.Log("Animal Count Increased");
    }

    public void decreaseAnimalCount()
    {
        animalCount--;
        Debug.Log("Animal Count Increased");
    }

    private void CheckGameConditions()
    {
        if (animalCount > maxAnimalCount)
        {
            Debug.Log("AnimalCount exceeded " + maxAnimalCount + " --- GAME OVER");
            _canvas.SetActive(true);
        }
    }

    public void ReloadScene()
    {
        _canvas.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
