using UnityEditor.SearchService;
using UnityEngine;

public class AnimalSpawnerScript : MonoBehaviour
{
    public GameObject prefabAnimal;
    public Sprite animalSprite;

    void Start()
    {

    }

    public void SpawnAnimal()
    {
        GameObject spawnedAnimal = Instantiate(prefabAnimal, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = spawnedAnimal.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animalSprite;
        Debug.Log("Animal " + animalSprite.name + " spawned");
    }
}
