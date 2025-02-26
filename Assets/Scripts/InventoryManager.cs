using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("----------Texts----------")]
    [SerializeField] private TextMeshProUGUI numberOfSeeds;
    [SerializeField] private TextMeshProUGUI numberOfGrass;
    [SerializeField] private TextMeshProUGUI numberOfMeat;
    [SerializeField] private TextMeshProUGUI numberOfManure;
    [SerializeField] private TextMeshProUGUI numberOfLogs;
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
    }

    private void Start()
    {
        UpdateText();
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
