using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.Rendering.Universal;
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
    public GameObject settingsManager;
    public GameSceneManager gameSceneManager;
    public AnimalSpawnerScript bearSpawner;
    [SerializeField] private int maxWinAnimalCount = 7;

    public SoundManager soundManager;

    private bool isLightActive = true;
    private List<Light2D> lights = new List<Light2D>();
    [SerializeField] private float lightTransitionTime = 180f;


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

        foreach (Light2D light in FindObjectsByType<Light2D>(FindObjectsSortMode.None))
        {
            if (light != null)
            {
                lights.Add(light);
                Debug.Log("Light " + light.name + " added");
            }
            else
            {
                Debug.Log("Light was supposedly null");
            }
            light.intensity = light.lightType == Light2D.LightType.Global ? 1 : 0;
        }
    }

    void Update()
    {
        if (soundManager == null)
        {
            soundManager = FindFirstObjectByType<SoundManager>();
        }
        if (end) return;
        if (Input.GetKeyDown(KeyCode.Escape)) ShowEscMenu();
        if (stop) return;
        time = (time + Time.deltaTime * 10) % maxTime;
        timer.text = String.Format("{0:00}:{1:00}", (int)(time / 60), (int)time % 60);
        CheckTime();
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
        ToggleLights();
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
        ToggleLights();
        soundManager.GetComponent<SoundManager>().PlayMainTheme();
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

    private void ToggleLights()
    {
        isLightActive = !isLightActive;
        StartCoroutine(FadeLights());
    }

    private IEnumerator FadeLights()
    {
        float startTime = 0f;
        while (startTime < lightTransitionTime)
        {

            foreach (Light2D light in lights)
            {
                if (light.lightType != Light2D.LightType.Global)
                {
                    light.intensity = isLightActive ? Mathf.Lerp(1, 0, startTime / lightTransitionTime) : Mathf.Lerp(0, 1, startTime / lightTransitionTime);
                    //Debug.Log("Light aint global, so Im turning it on");
                }
                else
                {
                    light.intensity = isLightActive ? Mathf.Lerp(0, 1, startTime / lightTransitionTime) : Mathf.Lerp(1, 0, startTime / lightTransitionTime);
                    //Debug.Log("Light is global, so Im turning it off"); ;
                }
            }
            startTime += Time.deltaTime;
            yield return null;
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
        if (GameObject.FindGameObjectsWithTag("Animal").Count() >= maxWinAnimalCount)
        {
            StopStartGame(true);
            end = true;
            gameSceneManager.GetComponent<GameSceneManager>().LoadCreditsScene();
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

    public void MuteEnableMusic()
    {
        soundManager.GetComponent<SoundManager>().MuteEnableMusic();
        settingsManager.GetComponent<SettingsManager>().UpdateSettingsText();
    }

    public void MuteEnableSFX()
    {
        soundManager.GetComponent<SoundManager>().MuteEnableSFX();
        settingsManager.GetComponent<SettingsManager>().UpdateSettingsText();
    }
}
