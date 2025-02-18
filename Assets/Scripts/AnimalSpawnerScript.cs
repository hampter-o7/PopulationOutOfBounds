using UnityEditor.SearchService;
using UnityEngine;

public class AnimalSpawnerScript : MonoBehaviour
{
    public GameObject prefabAnimal;
    public Sprite cowSprite;
    void Start()
    {
        SpawnAnimal();
    }

    void Update()
    {

    }

    void SpawnAnimal()
    {
        GameObject spawnedAnimal = Instantiate(prefabAnimal, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = spawnedAnimal.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cowSprite;
    }
}
