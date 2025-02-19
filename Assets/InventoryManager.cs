using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI numberOfSeeds;
    [SerializeField] TextMeshProUGUI numberOfGrass;
    [SerializeField] TextMeshProUGUI numberOfMeat;

    public static int seeds { get; private set; }
    public static int grass { get; private set; }
    public static int meat { get; private set; }
    public enum Food {
        seeds,
        grass,
        meat,
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        numberOfSeeds.text = ": " + seeds;
        numberOfGrass.text = ": " + grass;
        numberOfMeat.text = ": " + meat;
    }

    public void IncreaseFood(Food food)
    {
        switch (food)
        {
            case Food.seeds:
                seeds++;
                break;
            case Food.grass:
                grass++;
                break;
            case Food.meat:
                meat++;
                break;

        }
    }

    public void DecreaseFood(Food food)
    {
        switch (food)
        {
            case Food.seeds:
                seeds--;
                break;
            case Food.grass:
                grass--;
                break;
            case Food.meat:
                meat--;
                break;

        }
    }
}
