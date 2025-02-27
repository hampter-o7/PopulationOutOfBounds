using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("----------Texts----------")]
    [SerializeField] private TextMeshProUGUI chickenCountText;
    [SerializeField] private TextMeshProUGUI cowCountText;
    [SerializeField] private TextMeshProUGUI sheepCountText;
    [SerializeField] private TextMeshProUGUI foxCountText;
    [SerializeField] private TextMeshProUGUI wolfCountText;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI animalDeathText;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [Header("----------Objects----------")]
    [SerializeField] private GameObject animalDeathCanvas;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject escMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject keybindingsMenu;
    [SerializeField] private GameObject spawnAnimalButtons;
    [SerializeField] private GameObject tutorialCanvas;
    [Header("----------Managers----------")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private LightManager lightManager;
    [SerializeField] private GameSceneManager gameSceneManager;
    [SerializeField] private AnimalSpawnerScript bearSpawner;
    private SoundManager soundManager;
    [Header("----------Values----------")]
    [SerializeField] private int maxWinAnimalCount = 100;
    [SerializeField] private float deathTextFadeDuration = 5;
    [SerializeField] private float time = 12 * 60;
    private readonly int maxTime = 24 * 60;
    private int night = 0;
    private bool end = false;
    private bool stop = false;
    private bool isNight = false;
    private bool escMenuActive = false;

    public int GetNightNumber()
    {
        return night;
    }

    public int GetTime()
    {
        return (int)time;
    }

    public bool GetStop()
    {
        return stop;
    }

    public bool GetEnd()
    {
        return end;
    }

    public void CheckGameConditions()
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        if (animals.Count() >= maxWinAnimalCount)
        {
            stop = true;
            StopStartGame();
            end = true;
            gameSceneManager.LoadCreditsScene();
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
        // if (allAnimals.Count(animal => animal == 0) > 2)
        // {
        //     stop = true;
        //     StopStartGame();
        //     end = true;
        //     retryButton.SetActive(true);
        // }
    }

    public void ReloadScene()
    {
        soundManager.PlayMainTheme();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        inventoryManager = inventoryManager.GetComponent<InventoryManager>();
        settingsManager = settingsManager.GetComponent<SettingsManager>();
        lightManager = lightManager.GetComponent<LightManager>();
        gameSceneManager = gameSceneManager.GetComponent<GameSceneManager>();
        soundManager = FindFirstObjectByType<SoundManager>();
        StartCoroutine(SetAllButtonClicks());
        if (Tutorial.isTutorial) TutorialGame();
    }

    private void Update()
    {
        if (Tutorial.isTutorial)
        {
            CheckMouseClicks();
            return;
        }
        if (end) return;
        if (Input.GetKeyDown(KeyCode.Escape)) ShowEscMenu();
        if (stop || escMenuActive) return;
        time = (time + Time.deltaTime * 10) % maxTime;
        timer.text = String.Format("{0:00}:{1:00}", (int)(time / 60), (int)time % 60);
        CheckTime();
    }

    private void TutorialGame()
    {
        stop = true;
        StopStartGame();
        tutorialCanvas.SetActive(true);
    }

    private IEnumerator SetAllButtonClicks()
    {
        yield return new WaitForSeconds(0.2f);
        soundManager = FindFirstObjectByType<SoundManager>();
        SetAllAnimalButtonsText();
    }

    private void SetAllAnimalButtonsText()
    {
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
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
    }

    private void CheckTime()
    {
        if (time < 6 * 60 || time > 22 * 60)
        {
            if (GameObject.FindGameObjectWithTag("Bear") == null) StartNight();
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Bear") != null) StartDay();
            CheckMouseClicks();
        }
    }

    private void StartNight()
    {
        night++;
        isNight = true;
        lightManager.ToggleLights();
        spawnAnimalButtons.GetComponentsInChildren<Button>().ToList().ForEach(button => button.interactable = false);
        soundManager.PlayBearTheme();
        bearSpawner.SpawnAnimal(false);
    }

    private void StartDay()
    {
        isNight = false;
        lightManager.ToggleLights();
        spawnAnimalButtons.GetComponentsInChildren<Button>().ToList().ForEach(button => button.interactable = true);
        soundManager.PlayMainTheme();
        Destroy(GameObject.FindGameObjectWithTag("Bear"));
        soundManager.PlaySFX(3);
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject animal in animals) animal.GetComponent<AnimalScript>().SetHasDestPoint(false);
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
                if (spriteRenderer.bounds.Contains(mousePosition)) animal.GetComponent<AnimalScript>().SendIntoFence();
            }
            foreach (GameObject poop in poops)
            {
                SpriteRenderer spriteRenderer = poop.GetComponent<SpriteRenderer>();
                if (spriteRenderer.bounds.Contains(mousePosition))
                {
                    poop.SetActive(false);
                    Destroy(poop);
                    inventoryManager.ChangeManureValue(1);
                }
            }
        }
    }

    private void ShowEscMenu()
    {
        escMenuActive = !escMenuActive;
        escMenu.SetActive(escMenuActive);
        StopStartGame();
        settingsMenu.SetActive(false);
        keybindingsMenu.SetActive(false);
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
        Debug.Log(killer + " " + killed);
        string animalDeathString = "The " + killerName + " grew strong and the " + killedName + " was " + (killer == 0 ? "pecked" : killer == 1 || killer == 2 ? "stomped" : "devoured") + "!";
        StartCoroutine(UpdateAnimalDeathText(animalDeathString));
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

    private IEnumerator UpdateAnimalDeathText(string text)
    {
        animalDeathText.text = text;
        animalDeathCanvas.SetActive(true);
        animalDeathText.alpha = 1;
        float elapsedTime = 0;
        while (elapsedTime < deathTextFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            animalDeathText.alpha = Mathf.Lerp(1, 0, elapsedTime / deathTextFadeDuration);
            yield return null;
        }
        animalDeathText.alpha = 0;
    }

    private void StopStartGame()
    {
        bool isStop = stop || escMenuActive;
        if (GameObject.FindGameObjectWithTag("Bear") != null) GameObject.FindGameObjectWithTag("Bear").GetComponent<BearAnimalScript>().SetStop(isStop);
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject animal in animals) animal.GetComponent<AnimalScript>().SetStop(isStop);
        spawnAnimalButtons.GetComponentsInChildren<Button>().ToList().ForEach(button => button.interactable = !(isStop || isNight));
    }
}
