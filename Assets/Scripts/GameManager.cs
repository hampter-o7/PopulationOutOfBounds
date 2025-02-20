using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI animalCountText;
    public TextMeshProUGUI timer;
    public Tilemap fences;
    public Tile replacementFence;

    public GameObject retryButton;
    public GameObject spawnAnimalButtons;
    public GameObject inventoryManager;
    public AnimalSpawnerScript bearSpawner;
    [SerializeField] private int maxAnimalCount = 20;

    bool stop = false;
    private float time = 21 * 60;
    private int maxTime = 60 * 24;

    public int animalCount;

    private Dictionary<Vector3Int, TileBase> removedFences = new();
    private Dictionary<Vector3Int, TileBase> tempRemovedFences = new();

    private readonly Dictionary<Vector3Int, TileBase> originalRemovedFences = new()
    {
        {new(-5, -1, 0), null},
        {new(-5, -2, 0), null},
        {new(-2, -11, 0), null},
        {new(-2, -12, 0), null},
        {new(8, 0, 0), null},
        {new(8, 1, 0), null},
        {new(3, 8, 0), null},
        {new(-3, 11, 0), null},
    };
    void Start()
    {
        removedFences.AddRange(originalRemovedFences);
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
                foreach (Vector3Int position in removedFences.Keys)
                {
                    RemoveAddFence(true, position, false);
                }
                removedFences.Clear();
                removedFences.AddRange(tempRemovedFences);
                tempRemovedFences.Clear();
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Bear") != null)
            {
                inventoryManager.GetComponent<InventoryManager>().AddDailyResources();
                Destroy(GameObject.FindGameObjectWithTag("Bear"));
                foreach (Vector3Int position in removedFences.Keys)
                {
                    RemoveAddFence(false, position, false);
                }
                removedFences.Clear();
                removedFences.AddRange(originalRemovedFences);
                GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
                foreach (GameObject animal in animals)
                {
                    animal.GetComponent<AnimalScript>().hasDestPoint = false;
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

    public void RemoveAddFence(bool isRemoveFence, Vector3Int location, bool isBearBraking)
    {
        if (isRemoveFence)
        {
            if (isBearBraking)
            {
                removedFences.Add(location, fences.GetTile(location));
                fences.SetTile(location, null);
                return;
            }
            else
            {
                tempRemovedFences[location] = fences.GetTile(location);

            }
        }
        fences.SetTile(location, removedFences[location]);
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
