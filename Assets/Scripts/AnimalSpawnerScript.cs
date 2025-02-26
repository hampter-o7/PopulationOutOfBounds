using System;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalSpawnerScript : MonoBehaviour
{
    [Header("----------Game Objects----------")]
    [SerializeField] private GameObject prefabAnimal;
    [Header("----------Sprite----------")]
    [SerializeField] private Sprite animalSprite;
    [Header("----------Managers----------")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameManager gameManager;
    private readonly Vector3[] bearSpawningPositions =  {
        new(-0.442f, 8.703f, 0),
        new(-15.28f, -1.31f, 0),
        new(3.56f, -11.45f, 0),
        new(17.56f, -0.17f, 0),
    };

    public void SpawnAnimal(bool isStart)
    {
        bool doSpawn = true;
        if (Tutorial.isTutorial)
        {
            if (isStart) return;
            gameManager.AdvanceTutorial(animalSprite.name.Equals("Chicken") ? 1 : 0);
            doSpawn = animalSprite.name.Equals("Chicken");
        }
        if (!isStart && doSpawn)
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
        if (spawnedAnimal.GetComponent<AnimalScript>() != null) spawnedAnimal.GetComponent<AnimalScript>().SetSpawnPoint(transform.position);
        if (Tutorial.isTutorial) return;
        if (!isStart) gameManager.GetComponent<GameManager>().CheckGameConditions();
    }

    private void Start()
    {
        if (!animalSprite.name.Equals("Bear")) SpawnAnimal(true);
    }
}
