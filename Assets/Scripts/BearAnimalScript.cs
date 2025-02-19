using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BearAnimalScript : MonoBehaviour
{
    private Sprite image;
    public SpriteRenderer sr;
    private bool isFlipped = false;
    public Tilemap ground;
    public Tilemap fence;
    Vector3 destPoint;
    //bool hasDestPoint;
    private float waitTimeLeft = 0;
    [SerializeField] float range = 10;
    [SerializeField] float movementSpeed = 1;
    [SerializeField] float waitTime = 1;
    public bool stop = false;
    private float distanceToTargetAnimal;
    private float minimalDistanceToTargetAnimal = float.MaxValue;
    private float tempDistanceToTargetAnimal;
    private GameObject prey;

    [SerializeField] List<GameObject> targetAnimals = new List<GameObject>();

    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        fence = GameObject.Find("Fence").GetComponent<Tilemap>();
    }

    void Update()
    {
        AddTargetAnimalsToList();

        foreach(GameObject targetAnimal in targetAnimals)
        {
            tempDistanceToTargetAnimal = Vector2.Distance(transform.position, targetAnimal.transform.position);
            if (tempDistanceToTargetAnimal < minimalDistanceToTargetAnimal)
            {
                prey = targetAnimal;
                minimalDistanceToTargetAnimal = tempDistanceToTargetAnimal;
            }
        }

        if (stop)
        {
            return;
        }
        if (waitTimeLeft <= 0)
        {
            AnimalMovement();
        }
        else
        {
            waitTimeLeft -= Time.deltaTime;
        }
    }

    public void AnimalMovement()
    {
        SearchForDest();
        transform.position = Vector3.MoveTowards(transform.position, destPoint, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destPoint) < 1)
        {
            //hasDestPoint = false;
            KillAnimal(prey);
            waitTimeLeft += waitTime;
        }
    }

    void SearchForDest()
    {
        if (prey != null)
        {
            destPoint = prey.transform.position;
            Vector3 startPoint = transform.position;
            Vector3Int groundDest = ground.WorldToCell(destPoint);

            if (ground.HasTile(groundDest) && IsPathClear(startPoint, destPoint))
            {
                //hasDestPoint = true;
                bool wasFlipped = isFlipped;
                isFlipped = destPoint.x < startPoint.x;
                if (wasFlipped != isFlipped)
                {
                    sr.flipX = !sr.flipX;
                }
            }
        }
        else
        {
            Debug.Log("There is no prey for the mighty bear");
        }
    }

    bool IsPathClear(Vector3 start, Vector3 end)
    {
        float steps = range * range;
        for (int i = 0; i <= steps; i++)
        {
            Vector3 point = Vector3.Lerp(start, end, i / (float)steps);
            Vector3Int fenceCheck = fence.WorldToCell(point);

            if (fence.HasTile(fenceCheck))
            {
                return false;
            }
        }
        return true;
    }

    public void AddTargetAnimalsToList()
    {
        foreach (GameObject targetAnimal in GameObject.FindGameObjectsWithTag("Animal"))
        {
            if (!targetAnimals.Contains(targetAnimal))
            {
                targetAnimals.Add(targetAnimal);
                Debug.Log("Animal " + targetAnimal.name + " added");
            }
        }
    }

    private void KillAnimal(GameObject prey)
    {
        targetAnimals.Remove(prey);
        Destroy(prey);
        minimalDistanceToTargetAnimal = float.MaxValue;
    }
}
