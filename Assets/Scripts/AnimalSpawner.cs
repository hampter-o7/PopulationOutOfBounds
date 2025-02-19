using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    GameObject parent;
    [SerializeField] GameObject animal;
    //[SerializeField] AnimalSpawnerScript animalSpawnerScript;
    void Start()
    {
        parent = transform.parent?.gameObject;
    }

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
