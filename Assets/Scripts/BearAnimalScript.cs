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
    [SerializeField] float movementSpeed = 1;
    public GameObject gameManager;
    public bool stop = false;
    private Vector3 lastPosition;
    private float stuckTimer = 0;
    private float stuckTimerMax = 5;
    private float minimalDistanceToTargetAnimal = float.MaxValue;
    private float tempDistanceToTargetAnimal;
    private GameObject prey = null;

    [SerializeField] List<GameObject> targetAnimals = new List<GameObject>();

    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        fence = GameObject.Find("Fence").GetComponent<Tilemap>();
        gameManager = GameObject.Find("GameManager");
        lastPosition = transform.position;
    }

    void Update()
    {
        if (stop)
        {
            return;
        }
        AddTargetAnimalsToList();
        foreach (GameObject targetAnimal in targetAnimals)
        {
            tempDistanceToTargetAnimal = Vector2.Distance(transform.position, targetAnimal.transform.position);
            if (tempDistanceToTargetAnimal < minimalDistanceToTargetAnimal)
            {
                prey = targetAnimal;
                minimalDistanceToTargetAnimal = tempDistanceToTargetAnimal;
            }
        }
        AnimalMovement();
    }

    public void AnimalMovement()
    {
        if (prey == null)
        {
            // Debug.Log("There is no prey for the mighty bear");
            return;
        }
        SearchForDest();
        lastPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, destPoint, movementSpeed * Time.deltaTime);
        Vector3Int fenceCheck = fence.WorldToCell(transform.position);
        if (fence.HasTile(fenceCheck))
        {
            transform.position = lastPosition;
            stuckTimer += Time.deltaTime;
            if (stuckTimer > stuckTimerMax)
            {
                gameManager.GetComponent<GameManager>().RemoveAddFence(true, fenceCheck, true);
            }
        }
        else
        {
            stuckTimer = 0;
        }
        if (Vector3.Distance(transform.position, destPoint) < 0.5)
        {
            KillAnimal(prey);
        }
    }

    void SearchForDest()
    {
        destPoint = prey.transform.position;
        Vector3 startPoint = transform.position;
        Vector3Int groundDest = ground.WorldToCell(destPoint);

        if (ground.HasTile(groundDest))
        {
            bool wasFlipped = isFlipped;
            isFlipped = destPoint.x < startPoint.x;
            if (wasFlipped != isFlipped)
            {
                sr.flipX = !sr.flipX;
            }
        }
    }

    public void AddTargetAnimalsToList()
    {
        foreach (GameObject targetAnimal in GameObject.FindGameObjectsWithTag("Animal"))
        {
            if (!targetAnimals.Contains(targetAnimal))
            {
                targetAnimals.Add(targetAnimal);
                // Debug.Log("Animal " + targetAnimal.name + " added");
            }
        }
    }

    private void KillAnimal(GameObject prey)
    {
        gameManager.GetComponent<GameManager>().DecreaseAnimalCount();
        targetAnimals.Remove(prey);
        Destroy(prey);
        prey = null;
        minimalDistanceToTargetAnimal = float.MaxValue;
    }
}
