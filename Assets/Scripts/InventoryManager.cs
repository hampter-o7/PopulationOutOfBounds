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
        updateText();
    }

    void Update()
    {

    }

    private void updateText()
    {
        numberOfSeeds.text = seeds.ToString();
        numberOfGrass.text = grass.ToString();
        numberOfMeat.text = meat.ToString();
    }

    public void addDailyResources()
    {
        seeds += 5;
        grass += 5;
        meat += 5;
        updateText();
    }
}
