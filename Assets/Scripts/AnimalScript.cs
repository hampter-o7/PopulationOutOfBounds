using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalScript : MonoBehaviour
{
    public SpriteRenderer sr;
    private bool isFlipped = false;
    public Tilemap ground;
    public Tilemap fence;
    public GameManager gameManager;
    public GameObject poopPrefab;
    public Vector3 spawnPoint;
    Vector3 destPoint;
    public bool hasDestPoint;
    private float waitTimeLeft = 0;
    [SerializeField] float range = 10;
    [SerializeField] float movementSpeed = 1;
    [SerializeField] float waitTime = 1;
    public bool stop = false;
    private bool didPoopThisNight = false;
    private float poopTime = 1;

    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        fence = GameObject.Find("Fence").GetComponent<Tilemap>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        poopPrefab = Resources.Load<GameObject>("Poop");
    }

    void Update()
    {
        if (stop) return;
        CheckIfCanPoop();
        if (waitTimeLeft <= 0) AnimalMovement();
        else waitTimeLeft -= Time.deltaTime;
    }

    public void AnimalMovement()
    {
        if (!hasDestPoint) SearchForDest();
        if (hasDestPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPoint, movementSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, destPoint) < 1)
        {
            hasDestPoint = false;
            waitTimeLeft += waitTime;
        }
    }

    private void CheckIfCanPoop()
    {
        poopTime -= Time.deltaTime;
        if (poopTime > 0) return;
        poopTime = 1;
        if (!didPoopThisNight && (gameManager.time < 6 * 60))
        {
            int max = (int)(6 * 60 / gameManager.time);
            int rand = Random.Range(0, max);
            if (rand + 1 == max)
            {
                didPoopThisNight = true;
                GameObject spawnedAnimal = Instantiate(poopPrefab, transform.position, transform.rotation);
            }
        }
    }

    void SearchForDest()
    {
        float x = UnityEngine.Random.Range(-range, range);
        float y = UnityEngine.Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        Vector3 startPoint = transform.position;
        Vector3Int groundDest = ground.WorldToCell(destPoint);

        if (ground.HasTile(groundDest) && IsPathClear(startPoint, destPoint))
        {
            hasDestPoint = true;
            bool wasFlipped = isFlipped;
            isFlipped = destPoint.x < startPoint.x;
            if (wasFlipped != isFlipped)
            {
                sr.flipX = !sr.flipX;
            }
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

    public void SendIntoFence()
    {
        transform.position = spawnPoint;
        hasDestPoint = false;
    }
}
