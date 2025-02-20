using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _animalCountText;
    [SerializeField] GameObject retryButton;
    [SerializeField] GameObject spawnAnimalButtons;
    [SerializeField] private int maxAnimalCount = 20;

    public int animalCount { get; private set; }
    void Start()
    {

    }

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
            if (GameObject.FindGameObjectWithTag("Bear") != null)
            {
                GameObject.FindGameObjectWithTag("Bear").GetComponent<BearAnimalScript>().stop = true;
            }
            GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");

            foreach (GameObject animal in animals)
            {
                animal.GetComponent<AnimalScript>().stop = true;
            }
            Debug.Log("AnimalCount exceeded " + maxAnimalCount + " --- GAME OVER");
            retryButton.SetActive(true);
            spawnAnimalButtons.SetActive(false);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
