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
        if (spawnedAnimal.GetComponent<AnimalScript>() != null)
        {
            spawnedAnimal.GetComponent<AnimalScript>().spawnPoint = transform.position;
        }
        // Debug.Log("Animal " + animalSprite.name + " spawned");
    }
}
