using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalScript : MonoBehaviour
{
    private Sprite image;
    public Tilemap ground;
    public Tilemap fence;
    Vector3 destPoint;
    bool hasDestPoint;
    [SerializeField] float range = 10;
    [SerializeField] float movementSpeed;

    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        fence = GameObject.Find("Fence").GetComponent<Tilemap>();
    }

    void Update()
    {
        AnimalMovement();
    }

    public void AnimalMovement()
    {
        if (!hasDestPoint) SearchForDest();
        if (hasDestPoint) transform.position = Vector3.MoveTowards(transform.position, destPoint, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destPoint) < 1) hasDestPoint = false;
    }

    void SearchForDest()
    {
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        Vector3 startPoint = transform.position;
        Vector3Int groundDest = ground.WorldToCell(destPoint);

        if (ground.HasTile(groundDest) && IsPathClear(startPoint, destPoint))
        {
            hasDestPoint = true;
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

}
