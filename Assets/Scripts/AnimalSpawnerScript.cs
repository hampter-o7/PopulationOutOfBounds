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

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SpawnAnimal(cowSprite);
        }
    }

    public void SpawnAnimal(Sprite animalSprite)
    {
        GameObject spawnedAnimal = Instantiate(prefabAnimal, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = spawnedAnimal.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animalSprite;
        Debug.Log("Animal " + animalSprite.name + " spawned");
    }
}
