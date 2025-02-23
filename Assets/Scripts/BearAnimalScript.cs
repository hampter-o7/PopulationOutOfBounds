using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BearAnimalScript : MonoBehaviour
{
    public SpriteRenderer sr;
    private bool isFlipped = false;
    public Tilemap ground;
    public Tilemap fence;
    Vector3 destPoint;
    [SerializeField] float movementSpeed = 2;
    public GameManager gameManager;
    public InventoryManager inventoryManager;
    public bool stop = false;
    private Vector3 lastPosition;
    private float stuckTimer = 0;
    private readonly float stuckTimerMax = 1;
    public float eatAnimalTimer = 0;
    private float minimalDistanceToTargetAnimal = float.MaxValue;
    private float tempDistanceToTargetAnimal;
    private GameObject prey = null;


    [SerializeField] List<GameObject> targetAnimals = new List<GameObject>();

    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        fence = GameObject.Find("Fence").GetComponent<Tilemap>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        lastPosition = transform.position;
    }

    void Update()
    {
        if (stop) return;
        if (eatAnimalTimer > 0)
        {
            eatAnimalTimer -= Time.deltaTime;
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
                gameManager.RemoveAddFence(true, fenceCheck, true);
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
        FindFirstObjectByType<SoundManager>().GetComponent<SoundManager>().PlaySFX(0);
        string spriteName = prey.GetComponent<SpriteRenderer>().sprite.name;
        int meatNum = spriteName.Equals("Cow") ? 5 : spriteName.Equals("Sheep") ? 4 : spriteName.Equals("Wolf") ? 3 : spriteName.Equals("Fox") ? 2 : 1;
        inventoryManager.ChangeMeatValue(meatNum);
        targetAnimals.Remove(prey);
        prey.SetActive(false);
        Destroy(prey);
        prey = null;
        minimalDistanceToTargetAnimal = float.MaxValue;
        gameManager.CheckGameConditions();
        eatAnimalTimer = 3;
    }
}
