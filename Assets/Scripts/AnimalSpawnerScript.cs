using UnityEditor.SearchService;
using UnityEngine;

public class AnimalSpawnerScript : MonoBehaviour
{
    public GameObject prefabAnimal;
    public Sprite cowSprite;
    public Sprite chickenSprite;
    public Sprite chicSprite;
    public Sprite foxSprite;
    public Sprite wolfSprite;
    public Sprite sheepSprite;


    void Start()
    {
        SpawnAnimal(cowSprite);
        SpawnAnimal(chickenSprite);
        SpawnAnimal(chicSprite);
        SpawnAnimal(foxSprite);
        SpawnAnimal(wolfSprite);
        SpawnAnimal(sheepSprite);
    }

    void Update()
    {

    }

    public void SpawnAnimal(Sprite animalSprite)
    {
        GameObject spawnedAnimal = Instantiate(prefabAnimal, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = spawnedAnimal.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animalSprite;
        Debug.Log("Animal " + animalSprite.name + " spawned");
    }
}
