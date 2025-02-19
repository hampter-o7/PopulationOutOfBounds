using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    GameObject parent;
    [SerializeField] GameObject animal;
    //[SerializeField] AnimalSpawnerScript animalSpawnerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent?.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SpawnAnimal(animal);
        }
    }

    public void SpawnAnimal(GameObject spawnedAnimal)
    {
        Instantiate(spawnedAnimal, transform.position, transform.rotation);
        Debug.Log("Spawned one " + spawnedAnimal.name);
    }
}
