using System;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalSpawnerScript : MonoBehaviour
{
    public GameObject prefabAnimal;
    public Sprite animalSprite;
    public InventoryManager inventoryManager;
    public GameManager gameManager;
    [SerializeField] int animalCost;

    private Vector3[] bearSpawningPositions =  {
        new(-0.442f, 8.703f, 0),
        new(-15.28f, -1.31f, 0),
        new(3.56f, -11.45f, 0),
        new(17.56f, -0.17f, 0),
    };

    void Start()
    {
        if (!animalSprite.name.Equals("Bear")) SpawnAnimal(true);
    }

    public void SpawnAnimal(bool isStart)
    {
        bool doSpawn = true;
        if (!isStart)
        {
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
        }
        if (!doSpawn) return;
        GameObject spawnedAnimal = Instantiate(prefabAnimal, animalSprite.name.Equals("Bear") ? bearSpawningPositions[UnityEngine.Random.Range(0, 4)] : transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = spawnedAnimal.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animalSprite;
        if (spawnedAnimal.GetComponent<AnimalScript>() != null)
        {
            spawnedAnimal.GetComponent<AnimalScript>().spawnPoint = transform.position;
        }
        if (!isStart) gameManager.GetComponent<GameManager>().CheckGameConditions();
    }
}
