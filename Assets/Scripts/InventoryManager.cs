using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    public TextMeshProUGUI numberOfSeeds;
    public TextMeshProUGUI numberOfGrass;
    public TextMeshProUGUI numberOfMeat;



    [SerializeField] private int seeds;
    [SerializeField] private int grass;
    [SerializeField] private int meat;


    void Start()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        numberOfSeeds.text = seeds.ToString();
        numberOfGrass.text = grass.ToString();
        numberOfMeat.text = meat.ToString();
    }

    public void AddDailyResources()
    {
        seeds += 7;
        grass += 9;
        UpdateText();
    }

    public void AddAnimalMeat(int meatNum)
    {
        meat += meatNum;
        UpdateText();
    }
}
