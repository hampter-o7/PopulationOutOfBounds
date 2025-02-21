using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI animalCountText;
    public TextMeshProUGUI timer;
    public Tilemap fences;
    public Tile replacementFence;

    public GameObject retryButton;
    public GameObject escMenu;
    public GameObject settingsMenu;
    public GameObject spawnAnimalButtons;
    public GameObject inventoryManager;
    public AnimalSpawnerScript bearSpawner;
    [SerializeField] private int maxAnimalCount = 20;

    private SoundManager soundManager;

    bool stop = false;
    bool end = false;
    private float time = 21 * 60;
    private readonly int maxTime = 60 * 24;

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
        UpdateAnimalCountText();
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        if (end) return;
        if (Input.GetKeyDown(KeyCode.Escape)) ShowEscMenu();
        if (stop) return;
        time = (time + Time.deltaTime * 10) % maxTime;
        timer.text = String.Format("{0:00}:{1:00}", (int)(time / 60), time % 60);
        CheckTime();
        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>();
        }
    }

    void CheckTime()
    {
        if (time < 6 * 60 || time > 22 * 60)
        {
            if (GameObject.FindGameObjectWithTag("Bear") == null)
            {
                StartNight();
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Bear") != null)
            {
                StartDay();
            }

            CheckMouseClicks();
        }
    }

    private void StartNight()
    {
        soundManager.PlayBearTheme();
        bearSpawner.SpawnAnimal();
        foreach (Vector3Int position in removedFences.Keys)
        {
            RemoveAddFence(true, position, false);
        }
        removedFences.Clear();
        removedFences.AddRange(tempRemovedFences);
        tempRemovedFences.Clear();
    }

    private void StartDay()
    {
        soundManager.PlayMainTheme();
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

    private void CheckMouseClicks()
    {
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

    public void ShowEscMenu()
    {
        escMenu.SetActive(!stop);
        StopStartGame(!stop);
        settingsMenu.SetActive(false);
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

    public void UpdateAnimalCountText()
    {
        animalCountText.text = "Current animal count: " + GameObject.FindGameObjectsWithTag("Animal").Count();
        CheckGameConditions();
    }

    private void CheckGameConditions()
    {
        if (GameObject.FindGameObjectsWithTag("Animal").Count() > maxAnimalCount)
        {
            StopStartGame(true);
            end = true;
            // Debug.Log("AnimalCount exceeded " + maxAnimalCount + " --- GAME OVER");
            retryButton.SetActive(true);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StopStartGame(bool isStop)
    {
        if (GameObject.FindGameObjectWithTag("Bear") != null) GameObject.FindGameObjectWithTag("Bear").GetComponent<BearAnimalScript>().stop = isStop;
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject animal in animals)
        {
            animal.GetComponent<AnimalScript>().stop = isStop;
            spawnAnimalButtons.SetActive(!isStop);
            stop = isStop;
        }
    }

    public void MuteSound()
    {
        soundManager.MuteSound();
    }

    public void EnableSound()
    {
        soundManager.EnableSound();
    }
}
