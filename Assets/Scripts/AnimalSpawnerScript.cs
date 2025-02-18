using UnityEditor.SearchService;
using UnityEngine;

public class AnimalSpawnerScript : MonoBehaviour
{
    public GameObject prefabAnimal;
    public Sprite cowSprite;
    public Sprite chickenSprite;
    public Sprite chicSprite;

    void Start()
    {
        SpawnAnimal(cowSprite);
        SpawnAnimal(chickenSprite);
        SpawnAnimal(chicSprite);

    }

    void Update()
    {

    }

    void SpawnAnimal(Sprite animalSprite)
    {
        GameObject spawnedAnimal = Instantiate(prefabAnimal, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = spawnedAnimal.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animalSprite;
    }
}
