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
        bool doSpawn = true;
        switch (animalSprite.name)
        {
            case "Chicken":
                doSpawn = inventoryManager.ChangeSeedsValue(-3);
                break;
            case "Cow":
                doSpawn = inventoryManager.ChangeGrassValue(-7);
                break;
            case "Sheep":
                doSpawn = inventoryManager.ChangeGrassValue(-5);
                break;
            case "Fox":
                doSpawn = inventoryManager.ChangeMeatValue(-2);
                break;
            case "Wolf":
                doSpawn = inventoryManager.ChangeMeatValue(-4);
                break;
        }
        if (!doSpawn) return;
        GameObject spawnedAnimal = Instantiate(prefabAnimal, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = spawnedAnimal.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animalSprite;
        if (spawnedAnimal.GetComponent<AnimalScript>() != null)
        {
            spawnedAnimal.GetComponent<AnimalScript>().spawnPoint = transform.position;
        }


    }
}
