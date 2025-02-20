using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using UnityEngine.Tilemaps;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI animalCountText;
    public TextMeshProUGUI timer;
    public Tilemap fences;
    public Tile replacementFence;

    public GameObject retryButton;
    public GameObject spawnAnimalButtons;
    public AnimalSpawnerScript bearSpawner;
    [SerializeField] private int maxAnimalCount = 20;

    bool stop = false;
    private float time = 21 * 60;
    private int maxTime = 60 * 24;

    public int animalCount;

    private Vector3Int[] removeAddFences = {
        new Vector3Int(-5, -1, 0),
        new Vector3Int(-5, -2, 0),
        new Vector3Int(-2, -11, 0),
        new Vector3Int(-2, -12, 0),
        new Vector3Int(8, 0, 0),
        new Vector3Int(8, 1, 0),
        new Vector3Int(3, 8, 0),
        new Vector3Int(-3, 11, 0),
    };
    void Start()
    {
        animalCountText.text = "Current animal count: " + animalCount;
    }

    void Update()
    {
        if (stop) return;
        time = (time + Time.deltaTime * 10) % maxTime;
        timer.text = String.Format("{0:00}:{1:00}", (int)(time / 60), time % 60);
        CheckTime();
    }

    void CheckTime()
    {
        if (time < 6 * 60 || time > 22 * 60)
        {
            if (GameObject.FindGameObjectWithTag("Bear") == null)
            {
                bearSpawner.SpawnAnimal();
                foreach (Vector3Int position in removeAddFences)
                {
                    fences.SetTile(position, null);
                }
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Bear") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("Bear"));
                foreach (Vector3Int position in removeAddFences)
                {
                    fences.SetTile(position, replacementFence);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
                foreach (GameObject animal in animals)
                {
                    SpriteRenderer spriteRenderer = animal.GetComponent<SpriteRenderer>();
                    if (spriteRenderer.bounds.Contains(mousePosition))
                    {
                        animal.GetComponent<AnimalScript>().SendIntoFence();
                    }
                }
            }
        }
    }
    public void IncreaseAnimalCount()
    {
        animalCount++;
        animalCountText.text = "Current animal count: " + animalCount;
        CheckGameConditions();
    }

    public void DecreaseAnimalCount()
    {
        animalCount--;
        animalCountText.text = "Current animal count: " + animalCount;
        CheckGameConditions();
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
            // Debug.Log("AnimalCount exceeded " + maxAnimalCount + " --- GAME OVER");
            retryButton.SetActive(true);
            spawnAnimalButtons.SetActive(false);
            stop = true;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
