using UnityEditor.SearchService;
using UnityEngine;

public class AnimalSpawnerScript : MonoBehaviour
{
    public GameObject prefabAnimal;
    public Sprite animalSprite;
    public InventoryManager inventoryManager;
    [SerializeField] int animalCost;

    void Start()
    {
        if (!animalSprite.name.Equals("Bear")) SpawnAnimal();
    }

    public void SpawnAnimal()
    {
        GameObject spawnedAnimal = Instantiate(prefabAnimal, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = spawnedAnimal.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animalSprite;
        if (spawnedAnimal.GetComponent<AnimalScript>() != null)
        {
            spawnedAnimal.GetComponent<AnimalScript>().spawnPoint = transform.position;
        }

        switch (animalSprite.name)
        {
            case "Chicken":
                inventoryManager.ConsumeSeeds(3);
                break;
            case "Cow":
                inventoryManager.ConsumeGrass(7);
                break;
            case "Sheep":
                inventoryManager.ConsumeGrass(5);
                break;
            case "Fox":
                inventoryManager.ConsumeMeat(2);
                Debug.Log("Spawned Fox, consumed 2 meat");
                break;
            case "Wolf":
                inventoryManager.ConsumeMeat(4);
                break;
        }
    }
}
