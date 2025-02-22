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
    public TextMeshProUGUI chickenCountText;
    public TextMeshProUGUI cowCountText;
    public TextMeshProUGUI sheepCountText;
    public TextMeshProUGUI foxCountText;
    public TextMeshProUGUI wolfCountText;

    public TextMeshProUGUI timer;
    public Tilemap fences;
    public Tilemap ground;
    public Tile farmland;
    public Tile grass;
    public Tile seedsPlanted;
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

    private int timeToGrow = 60;

    public SoundManager soundManager;

    private bool isLightActive = true;
    private List<Light2D> lights = new List<Light2D>();
    [SerializeField] private float lightTransitionTime = 180f;


    bool stop = false;
    bool escMenuActive = false;
    bool end = false;
    public float time = 6 * 60;
    private readonly int maxTime = 24 * 60;

    private Dictionary<Vector3Int, TileBase> removedFences = new();
    private Dictionary<Vector3Int, TileBase> tempRemovedFences = new();
    private Dictionary<Vector3Int, float> seedsPlantedTiles = new();


    private readonly Dictionary<Vector3Int, TileBase> originalRemovedFences = new()
    {
        {new(-1, -1, 0), null},
        {new(-6, 2, 0), null},
        {new(-3, 8, 0), null},
        {new(4, 11, 0), null},
        {new(13, 3, 0), null},
        {new(13, 4, 0), null},
        {new(17, 0, 0), null},
        {new(18, 0, 0), null},
        {new(10, -8, 0), null},
        {new(6, -13, 0), null},
        {new(-4, -16, 0), null},
        {new(-5, -13, 0), null},
        {new(-10, -13, 0), null},

    };
    void Start()
    {
        removedFences.AddRange(originalRemovedFences);
        CheckGameConditions();

        foreach (Light2D light in FindObjectsByType<Light2D>(FindObjectsSortMode.None))
        {
            if (light != null)
            {
                lights.Add(light);
                // Debug.Log("Light " + light.name + " added");
            }
            else
            {
                // Debug.Log("Light was supposedly null");
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
        if (stop || escMenuActive) return;
        UpdateGrowingSeeds();
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
        spawnAnimalButtons.SetActive(false);
        soundManager.PlayBearTheme();
        bearSpawner.SpawnAnimal(false);
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
        spawnAnimalButtons.SetActive(true);
        soundManager.GetComponent<SoundManager>().PlayMainTheme();
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
        inventoryManager.GetComponent<InventoryManager>().AddDailyResources(animals.Count());
    }

    private void CheckMouseClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
            GameObject[] poops = GameObject.FindGameObjectsWithTag("Poop");
            foreach (GameObject animal in animals)
            {
                SpriteRenderer spriteRenderer = animal.GetComponent<SpriteRenderer>();
                if (spriteRenderer.bounds.Contains(mousePosition))
                {
                    animal.GetComponent<AnimalScript>().SendIntoFence();
                }
            }
            foreach (GameObject poop in poops)
            {
                SpriteRenderer spriteRenderer = poop.GetComponent<SpriteRenderer>();
                if (spriteRenderer.bounds.Contains(mousePosition))
                {
                    poop.SetActive(false);
                    Destroy(poop);
                    inventoryManager.GetComponent<InventoryManager>().ChangeManureValue(1);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = ground.WorldToCell(mousePosition);
            InventoryManager inventoryManagerSc = inventoryManager.GetComponent<InventoryManager>();
            string clickedTile = ground.GetTile(cellPosition).name;
            if (clickedTile.Equals("Farmable") && inventoryManagerSc.ChangeManureValue(-1)) ground.SetTile(cellPosition, farmland);
            if (clickedTile.Equals("Farmland") && inventoryManagerSc.ChangeSeedsValue(-1))
            {
                ground.SetTile(cellPosition, seedsPlanted);
                seedsPlantedTiles.Add(cellPosition, timeToGrow);
            }
            if (clickedTile.Equals("Grass"))
            {
                inventoryManagerSc.ChangeGrassValue(3);
                inventoryManagerSc.ChangeSeedsValue(3);
                ground.SetTile(cellPosition, farmland);
            }
        }
    }

    private void UpdateGrowingSeeds()
    {
        foreach (Vector3Int tile in seedsPlantedTiles.Keys.ToList())
        {
            seedsPlantedTiles[tile] -= Time.deltaTime;
            if (seedsPlantedTiles[tile] <= 0)
            {
                seedsPlantedTiles.Remove(tile);
                ground.SetTile(tile, grass);
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
                }
                else
                {
                    light.intensity = isLightActive ? Mathf.Lerp(0, 1, startTime / lightTransitionTime) : Mathf.Lerp(1, 0, startTime / lightTransitionTime);
                }
            }
            startTime += Time.deltaTime;
            yield return null;
        }
    }

    public void ShowEscMenu()
    {
        escMenuActive = !escMenuActive;
        escMenu.SetActive(escMenuActive);
        StopStartGame();
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

    public void CheckGameConditions()
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        if (animals.Count() >= maxWinAnimalCount)
        {
            stop = true;
            StopStartGame();
            end = true;
            gameSceneManager.GetComponent<GameSceneManager>().LoadCreditsScene();
        }
        int chickens = animals.Count(animal => animal.GetComponent<SpriteRenderer>().sprite.name.Equals("Chicken"));
        int cows = animals.Count(animal => animal.GetComponent<SpriteRenderer>().sprite.name.Equals("Cow"));
        int sheep = animals.Count(animal => animal.GetComponent<SpriteRenderer>().sprite.name.Equals("Sheep"));
        int foxes = animals.Count(animal => animal.GetComponent<SpriteRenderer>().sprite.name.Equals("Fox"));
        int wolfs = animals.Count(animal => animal.GetComponent<SpriteRenderer>().sprite.name.Equals("Wolf"));
        chickenCountText.text = chickens.ToString();
        cowCountText.text = cows.ToString();
        sheepCountText.text = sheep.ToString();
        foxCountText.text = foxes.ToString();
        wolfCountText.text = wolfs.ToString();

        int[] allAnimals = { chickens, cows, sheep, foxes, wolfs };
        if (CheckAllAnimalsCount(allAnimals, 0, 10) != -1) KillAllAnimalsWithName(0, CheckAllAnimalsCount(allAnimals, 0, 10));
        if (CheckAllAnimalsCount(allAnimals, 1, 10) != -1) KillAllAnimalsWithName(1, CheckAllAnimalsCount(allAnimals, 1, 10));
        if (CheckAllAnimalsCount(allAnimals, 2, 10) != -1) KillAllAnimalsWithName(2, CheckAllAnimalsCount(allAnimals, 2, 10));
        if (CheckAllAnimalsCount(allAnimals, 3, 10) != -1) KillAllAnimalsWithName(3, CheckAllAnimalsCount(allAnimals, 3, 10));
        if (CheckAllAnimalsCount(allAnimals, 4, 10) != -1) KillAllAnimalsWithName(4, CheckAllAnimalsCount(allAnimals, 4, 10));
        if (chickens != 0 && chickens * 3 <= foxes) KillAllAnimalsWithName(3, 0);
        if (sheep != 0 && sheep * 5 <= wolfs) KillAllAnimalsWithName(4, 2);
        if (allAnimals.Count(animal => animal == 0) > 3)
        {
            stop = true;
            StopStartGame();
            end = true;
            retryButton.SetActive(true);
        }
    }

    private int CheckAllAnimalsCount(int[] allAnimals, int animal, int maxCount)
    {
        for (int i = 0; i < allAnimals.Length; i++)
        {
            if (i == animal || allAnimals[i] == 0) continue;
            if (allAnimals[animal] >= allAnimals[i] * maxCount) return i;
        }
        return -1;
    }

    private void KillAllAnimalsWithName(int killer, int killed)
    {
        string killedName = killed == 0 ? "Chicken" : killed == 1 ? "Cow" : killed == 2 ? "Sheep" : killed == 3 ? "Fox" : "Wolf";
        string killerName = killer == 0 ? "Chicken" : killer == 1 ? "Cow" : killer == 2 ? "Sheep" : killer == 3 ? "Fox" : "Wolf";

        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        animals.ToList().ForEach(animal =>
        {
            if (animal.GetComponent<SpriteRenderer>().sprite.name.Equals(killedName))
            {
                animal.SetActive(false);
                Destroy(animal);
            }
        });
        CheckGameConditions();
    }

    public void ReloadScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        soundManager.GetComponent<SoundManager>().PlayMainTheme();
    }

    private void StopStartGame()
    {
        bool isStop = stop || escMenuActive;
        if (GameObject.FindGameObjectWithTag("Bear") != null) GameObject.FindGameObjectWithTag("Bear").GetComponent<BearAnimalScript>().stop = isStop;
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject animal in animals)
        {
            animal.GetComponent<AnimalScript>().stop = isStop;
            spawnAnimalButtons.SetActive(!isStop);
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
