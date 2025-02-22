using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    public TextMeshProUGUI numberOfSeeds;
    public TextMeshProUGUI numberOfGrass;
    public TextMeshProUGUI numberOfMeat;
    public TextMeshProUGUI numberOfManure;



    [SerializeField] private int seeds;
    [SerializeField] private int grass;
    [SerializeField] private int meat;
    [SerializeField] private int manure;


    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        numberOfSeeds.text = seeds.ToString();
        numberOfGrass.text = grass.ToString();
        numberOfMeat.text = meat.ToString();
        numberOfManure.text = manure.ToString();
    }

    public void AddDailyResources()
    {
        seeds += 7;
        grass += 9;
        UpdateText();
    }

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
}
