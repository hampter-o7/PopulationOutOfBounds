using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalScript : MonoBehaviour
{
    private Sprite image;
    public SpriteRenderer sr;
    private bool isFlipped = false;
    public Tilemap ground;
    public Tilemap fence;
    public Vector3 spawnPoint;
    Vector3 destPoint;
    bool hasDestPoint;
    private float waitTimeLeft = 0;
    [SerializeField] float range = 10;
    [SerializeField] float movementSpeed = 1;
    [SerializeField] float waitTime = 1;
    public bool stop = false;

    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        fence = GameObject.Find("Fence").GetComponent<Tilemap>();
    }

    void Update()
    {
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
        if (!hasDestPoint) SearchForDest();
        if (hasDestPoint) transform.position = Vector3.MoveTowards(transform.position, destPoint, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destPoint) < 1)
        {
            hasDestPoint = false;
            waitTimeLeft += waitTime;
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
