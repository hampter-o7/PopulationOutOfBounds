using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BearAnimalScript : MonoBehaviour
{
    [Header("----------Sprite Renderer----------")]
    [SerializeField] private SpriteRenderer sr;
    [Header("----------TileMaps----------")]
    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap fence;
    [Header("----------Managers----------")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private InventoryManager inventoryManager;
    private SoundManager soundManager;
    [Header("----------Values----------")]
    [SerializeField] private float movementSpeed = 2;
    [SerializeField] private float stuckTimerMax = 1;
    [SerializeField] private float eatTimerMax = 2;
    [SerializeField] private float eatTimerScaling = 5;
    private float minimalDistanceToTargetAnimal = float.MaxValue;
    private float tempDistanceToTargetAnimal;
    private float eatAnimalTimer = 0;
    private float stuckTimer = 0;
    private bool isFlipped = false;
    private bool stop = false;
    private GameObject prey = null;
    private Vector3 lastPosition;
    private Vector3 destPoint;

    [SerializeField] List<GameObject> targetAnimals = new List<GameObject>();

    public void SetStop(bool isStop)
    {
        stop = isStop;
    }
    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        fence = GameObject.Find("Fence").GetComponent<Tilemap>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        soundManager = FindFirstObjectByType<SoundManager>();
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
            if (targetAnimal == null) continue;
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
        if (prey == null) return;
        SearchForDest();
        lastPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, destPoint, gameManager.GetNightNumber() * movementSpeed * Time.deltaTime / 3);
        Vector3Int fenceCheck = fence.WorldToCell(transform.position);
        if (fence.HasTile(fenceCheck))
        {
            transform.position = lastPosition;
            stuckTimer += Time.deltaTime;
            if (stuckTimer > stuckTimerMax)
            {
                tileManager.AddOrRemoveFence(fenceCheck, false);
                soundManager.PlaySFX(3);
            }
        }
        else stuckTimer = 0;
        if (Vector3.Distance(transform.position, destPoint) < 0.5) KillAnimal(prey);
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
            if (wasFlipped != isFlipped) sr.flipX = !sr.flipX;
        }
    }

    public void AddTargetAnimalsToList()
    {
        foreach (GameObject targetAnimal in GameObject.FindGameObjectsWithTag("Animal")) if (!targetAnimals.Contains(targetAnimal)) targetAnimals.Add(targetAnimal);
    }

    private void KillAnimal(GameObject prey)
    {
        soundManager.PlaySFX(0);
        string spriteName = prey.GetComponent<SpriteRenderer>().sprite.name;
        int meatNum = spriteName.Equals("Cow") ? 5 : spriteName.Equals("Sheep") ? 4 : spriteName.Equals("Wolf") ? 3 : spriteName.Equals("Fox") ? 2 : 1;
        inventoryManager.ChangeMeatValue(meatNum);
        targetAnimals.Remove(prey);
        prey.SetActive(false);
        Destroy(prey);
        prey = null;
        minimalDistanceToTargetAnimal = float.MaxValue;
        gameManager.CheckGameConditions();
        eatAnimalTimer = eatTimerMax - (gameManager.GetNightNumber() / eatTimerScaling);
    }
}
