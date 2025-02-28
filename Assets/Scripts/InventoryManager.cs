using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("----------Managers----------")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TutorialManager tutorialManager;
    [Header("----------Texts----------")]
    [SerializeField] private TextMeshProUGUI numberOfSeeds;
    [SerializeField] private TextMeshProUGUI numberOfGrass;
    [SerializeField] private TextMeshProUGUI numberOfMeat;
    [SerializeField] private TextMeshProUGUI numberOfManure;
    [SerializeField] private TextMeshProUGUI numberOfLogs;
    [Header("----------Objects----------")]
    [SerializeField] private GameObject shadow;
    [SerializeField] private Sprite hammerSprite;
    [SerializeField] private Sprite axeSprite;
    [SerializeField] private Sprite hoeSprite;
    [SerializeField] private Sprite seedsSprite;
    [SerializeField] private Sprite scytheSprite;
    [SerializeField] private Sprite shovelSprite;
    [Header("----------Resources----------")]
    [SerializeField] private int seeds;
    [SerializeField] private int grass;
    [SerializeField] private int meat;
    [SerializeField] private int manure;
    [SerializeField] private int logs;
    [Header("----------Tool----------")]
    [SerializeField] private Button tool;
    private string selectedTool;

    public bool ChangeSeedsValue(int numSeeds)
    {
        if (seeds + numSeeds < 0) return false;
        seeds += numSeeds;
        UpdateText();
        return true;
    }
    public bool ChangeGrassValue(int numGrass)
    {
        if (grass + numGrass < 0) return false;
        grass += numGrass;
        UpdateText();
        return true;
    }

    public bool ChangeMeatValue(int numMeat)
    {
        if (meat + numMeat < 0) return false;
        meat += numMeat;
        UpdateText();
        return true;
    }

    public bool ChangeManureValue(int numManure)
    {
        if (manure + numManure < 0) return false;
        manure += numManure;
        UpdateText();
        return true;
    }
    public bool ChangeLogValue(int numLogs)
    {
        if (logs + numLogs < 0) return false;
        logs += numLogs;
        UpdateText();
        return true;
    }

    public string GetToolSelected()
    {
        return selectedTool;
    }

    public void SelectATool(Button clickedButton)
    {
        tool.image.sprite = clickedButton.image.sprite;
        selectedTool = tool.image.sprite.name;
        shadow.SetActive(selectedTool.Equals("axe"));
        if (Tutorial.isTutorial) tutorialManager.AdvanceTutorial(selectedTool.Equals("hammer") ? 1 : 2);
    }

    private void Start()
    {
        tutorialManager = tutorialManager.GetComponent<TutorialManager>();
        UpdateText();
    }

    private void Update()
    {
        SelectToolsWithKeyboardClicks();
    }

    private void SelectToolsWithKeyboardClicks()
    {
        string oldTool = tool.image.sprite.name;
        if (Input.GetKey(KeyCode.Alpha1)) tool.image.sprite = hammerSprite;
        if (Input.GetKey(KeyCode.Alpha2)) tool.image.sprite = axeSprite;
        if (Input.GetKey(KeyCode.Alpha3)) tool.image.sprite = hoeSprite;
        if (Input.GetKey(KeyCode.Alpha4)) tool.image.sprite = seedsSprite;
        if (Input.GetKey(KeyCode.Alpha5)) tool.image.sprite = scytheSprite;
        if (Input.GetKey(KeyCode.Alpha6)) tool.image.sprite = shovelSprite;
        if (oldTool == selectedTool) return;
        selectedTool = tool.image.sprite.name;
        shadow.SetActive(selectedTool.Equals("axe"));
        if (Tutorial.isTutorial) tutorialManager.AdvanceTutorial(selectedTool.Equals("hammer") ? 1 : 2);
    }

    private void UpdateText()
    {
        numberOfSeeds.text = seeds.ToString();
        numberOfGrass.text = grass.ToString();
        numberOfMeat.text = meat.ToString();
        numberOfManure.text = manure.ToString();
        numberOfLogs.text = logs.ToString();
    }
}
