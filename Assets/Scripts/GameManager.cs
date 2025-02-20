using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI animalCountText;
    public TextMeshProUGUI timer;

    public GameObject retryButton;
    public GameObject spawnAnimalButtons;
    public AnimalSpawnerScript bearSpawner;
    [SerializeField] private int maxAnimalCount = 20;

    private float time = 0;
    private int maxTime = 60 * 24;

    public int animalCount;
    void Start()
    {
        animalCountText.text = "Current animal count: " + animalCount;
    }

    void Update()
    {
        time = (time + Time.deltaTime * 10) % maxTime;
        timer.text = String.Format("{0:00}:{1:00}", (int)(time / 60), time % 60);
        CheckTime();
    }

    void CheckTime()
    {
        if (((int)time) / 60 < 6 || ((int)time) / 60 > 22)
        {
            if (GameObject.FindGameObjectWithTag("Bear") == null)
            {
                bearSpawner.SpawnAnimal();
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Bear") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("Bear"));
            }
        }
    }
    public void IncreaseAnimalCount()
    {
        animalCount++;
        animalCountText.text = "Current animal count: " + animalCount;
        CheckGameConditions();
        Debug.Log("Animal Count Increased");
    }

    public void DecreaseAnimalCount()
    {
        animalCount--;
        animalCountText.text = "Current animal count: " + animalCount;
        CheckGameConditions();
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
