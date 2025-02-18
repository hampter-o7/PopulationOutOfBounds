using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class AnimalScript : MonoBehaviour
{
    private Sprite image;
    public Tilemap ground;
    public Tilemap fence;
    Vector3 destPoint;
    bool hasDestPoint;
    [SerializeField] float range = 1;
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
        Vector3Int groundDest = ground.WorldToCell(destPoint);
        Vector3Int fenceDest = fence.WorldToCell(destPoint);
        if (ground.HasTile(groundDest) &&
        !fence.HasTile(fenceDest + Vector3Int.up) &&
        !fence.HasTile(fenceDest + Vector3Int.up + Vector3Int.left) &&
        !fence.HasTile(fenceDest + Vector3Int.up + Vector3Int.right))
        {
            hasDestPoint = true;
        }
    }
}
