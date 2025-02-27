using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    [Header("----------Managers----------")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TutorialManager tutorialManager;
    [Header("----------Tilemaps----------")]
    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap trees;
    [SerializeField] private Tilemap fences;
    [Header("----------Tiles----------")]
    [SerializeField] private TileBase[] groundTiles;
    [SerializeField] private TileBase[] treeTiles;
    [SerializeField] private TileBase[] fenceTiles;
    [Header("----------Objects----------")]
    [SerializeField] private RawImage shadow1;
    [SerializeField] private RawImage shadow2;
    [Header("----------Costs----------")]
    [SerializeField] private int fenceCost = 1;
    [SerializeField] private int fertilizationCost = 1;
    [SerializeField] private int seedsPlantedCost = 1;
    [Header("----------Profits----------")]
    [SerializeField] private int fenceLogReturn = 1;
    [SerializeField] private int seedsGainedFromHarvesting = 3;
    [SerializeField] private int grassGainedFromHarvesting = 3;
    [SerializeField] private int logsGainedFromChoppingTrees = 20;
    [SerializeField] private int logsGainedFromShovelingStumps = 3;
    [Header("----------Time to grow----------")]
    [SerializeField] private int seedsGrowthTimeInSeconds = 60;
    [SerializeField] private int seedsGrowthTimeInSecondsForTutorial = 5;
    [SerializeField] private int axeRechargeTimer = 10;
    private bool canUseAxe = true;
    private bool[,] isFence;
    private readonly int maxTreeChance = 30;
    private readonly int[] tileWeights = { 100, 20, 5, 5 };

    public void AddOrRemoveFence(Vector3Int position, bool isAdd)
    {
        if (Tutorial.isTutorial) tutorialManager.AdvanceTutorial(11);
        int x = position.x + isFence.GetLength(0) / 2;
        int y = position.y + isFence.GetLength(1) / 2;
        isFence[x, y] = isAdd;
        SetCorrectFenceAndFencesAround(x, y);
        FindFirstObjectByType<SoundManager>().PlaySFX(3);
    }

    private void Start()
    {
        inventoryManager = inventoryManager.GetComponent<InventoryManager>();
        gameManager = gameManager.GetComponent<GameManager>();
        tutorialManager = tutorialManager.GetComponent<TutorialManager>();
        isFence = new bool[ground.cellBounds.size.x, ground.cellBounds.size.y];
        CheckAllFences();
        SetTilesToRandom();
    }

    private void Update()
    {
        MouseClicks();
    }

    private void MouseClicks()
    {
        if (!Tutorial.isTutorial)
        {
            int currentTime = gameManager.GetTime();
            if (currentTime < 6 * 60 || currentTime > 22 * 60 || gameManager.GetStop() || gameManager.GetEnd() || gameManager.GetEscMenuActive()) return;
        }
        if (Input.GetMouseButton(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = ground.WorldToCell(mousePosition);
            switch (inventoryManager.GetToolSelected())
            {
                case "hammer":
                    UseHammer(cellPosition);
                    break;
                case "axe":
                    UseAxe(cellPosition);
                    break;
                case "hoe":
                    UseHoe(cellPosition);
                    break;
                case "seeds":
                    UseSeeds(cellPosition);
                    break;
                case "scythe":
                    UseScythe(cellPosition);
                    break;
                case "shovel":
                    UseShovel(cellPosition);
                    break;
            }
        }
    }

    private void UseHammer(Vector3Int position)
    {
        TileBase groundTile = ground.GetTile(position);
        if (groundTile == null || groundTile.Equals(groundTiles[4]) || groundTile.Equals(groundTiles[5]) || groundTile.Equals(groundTiles[6]) || trees.GetTile(position) != null || fences.GetTile(position) != null) return;
        if (inventoryManager.ChangeLogValue(-fenceCost)) AddOrRemoveFence(position, true);
    }

    private void UseAxe(Vector3Int position)
    {
        TileBase treeTile = trees.GetTile(position);
        if (treeTile == treeTiles[0] || treeTile == treeTiles[1])
        {
            if (!canUseAxe) return;
            inventoryManager.ChangeLogValue(logsGainedFromChoppingTrees);
            trees.SetTile(position, treeTile.Equals(treeTiles[0]) ? treeTiles[2] : treeTiles[3]);

            StartCoroutine(StartAxeRechargeTimer());
        }
        if (fences.GetTile(position) != null)
        {
            inventoryManager.ChangeLogValue(fenceLogReturn);
            AddOrRemoveFence(position, false);
        }
    }

    private IEnumerator StartAxeRechargeTimer()
    {
        canUseAxe = false;
        float time = 0;
        RectTransform shadowRect1 = shadow1.rectTransform;
        RectTransform shadowRect2 = shadow2.rectTransform;
        while (time < axeRechargeTimer)
        {
            if (gameManager.GetEscMenuActive())
            {
                yield return null;
                continue;
            }
            time += Time.deltaTime;
            if (inventoryManager.GetToolSelected().Equals("axe")) shadowRect1.sizeDelta = new Vector2(shadowRect1.sizeDelta.x, Mathf.Lerp(64, 0, time / axeRechargeTimer));
            shadowRect2.sizeDelta = new Vector2(shadowRect2.sizeDelta.x, Mathf.Lerp(32, 0, time / axeRechargeTimer));
            yield return null;
        }
        shadowRect1.sizeDelta = new Vector2(shadowRect1.sizeDelta.x, 0);
        shadowRect2.sizeDelta = new Vector2(shadowRect2.sizeDelta.x, 0);
        canUseAxe = true;
    }

    private void UseHoe(Vector3Int position)
    {
        TileBase groundTile = ground.GetTile(position);
        if (groundTile == null || groundTile.Equals(groundTiles[4]) || groundTile.Equals(groundTiles[5]) || groundTile.Equals(groundTiles[6]) || trees.GetTile(position) != null || fences.GetTile(position) != null) return;
        if (inventoryManager.ChangeManureValue(-fertilizationCost)) ground.SetTile(position, groundTiles[4]);
    }

    private void UseSeeds(Vector3Int position)
    {
        if (!ground.GetTile(position).Equals(groundTiles[4])) return;
        if (inventoryManager.ChangeSeedsValue(-seedsPlantedCost))
        {
            ground.SetTile(position, groundTiles[5]);
            StartCoroutine(SetToGrass(position));
        }
    }

    private IEnumerator SetToGrass(Vector3Int position)
    {
        float time = 0;
        while (time < (Tutorial.isTutorial ? seedsGrowthTimeInSecondsForTutorial : seedsGrowthTimeInSeconds))
        {
            if (gameManager.GetEscMenuActive())
            {
                yield return null;
                continue;
            }
            time += Time.deltaTime;
            yield return null;
        }
        ground.SetTile(position, groundTiles[6]);
    }

    private void UseScythe(Vector3Int position)
    {
        if (!ground.GetTile(position).Equals(groundTiles[6])) return;
        inventoryManager.ChangeSeedsValue(seedsGainedFromHarvesting);
        inventoryManager.ChangeGrassValue(grassGainedFromHarvesting);
        ground.SetTile(position, groundTiles[4]);
        if (Tutorial.isTutorial) tutorialManager.AdvanceTutorial(4);
    }
    private void UseShovel(Vector3Int position)
    {
        TileBase treeTile = trees.GetTile(position);
        if (treeTile == null || treeTile == treeTiles[0] || treeTile == treeTiles[1]) return;
        {
            inventoryManager.ChangeLogValue(logsGainedFromShovelingStumps);
            trees.SetTile(position, null);
        }
    }

    private void CheckAllFences()
    {
        int width = isFence.GetLength(0);
        int height = isFence.GetLength(1);
        foreach (Vector3Int position in fences.cellBounds.allPositionsWithin)
        {
            if (!fences.HasTile(position)) continue;
            isFence[position.x + width / 2, position.y + height / 2] = true;
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++) if (isFence[x, y]) SetCorrectFence(x, y);
        }
    }

    private void SetCorrectFenceAndFencesAround(int x, int y)
    {
        SetCorrectFence(x, y);
        if (x + 1 != isFence.GetLength(0)) SetCorrectFence(x + 1, y);
        if (x - 1 != -1) SetCorrectFence(x - 1, y);
        if (y + 1 != isFence.GetLength(1)) SetCorrectFence(x, y + 1);
        if (y - 1 != -1) SetCorrectFence(x, y - 1);
    }

    private void SetCorrectFence(int x, int y)
    {
        int rightFenceNum = -1;
        if (isFence[x, y])
        {
            bool isFenceOnRightUp = x + 1 != isFence.GetLength(0) && isFence[x + 1, y];
            bool isFenceOnRightDown = y - 1 != -1 && isFence[x, y - 1];
            bool isFenceOnLeftDown = x - 1 != -1 && isFence[x - 1, y];
            bool isFenceOnLeftUp = y + 1 != isFence.GetLength(1) && isFence[x, y + 1];
            int connectedFences = (isFenceOnRightUp ? 1 : 0) + (isFenceOnRightDown ? 1 : 0) + (isFenceOnLeftDown ? 1 : 0) + (isFenceOnLeftUp ? 1 : 0);
            switch (connectedFences)
            {
                case 0:
                    rightFenceNum = 0;
                    break;
                case 1:
                    rightFenceNum = isFenceOnRightUp ? 1 : isFenceOnRightDown ? 2 : isFenceOnLeftDown ? 3 : 4;
                    break;
                case 2:
                    rightFenceNum = isFenceOnRightUp ? isFenceOnRightDown ? 5 : isFenceOnLeftDown ? 6 : 7 : isFenceOnRightDown ? isFenceOnLeftDown ? 8 : 9 : 10;
                    break;
                case 3:
                    rightFenceNum = !isFenceOnRightUp ? 11 : !isFenceOnRightDown ? 12 : !isFenceOnLeftDown ? 13 : 14;
                    break;
                case 4:
                    rightFenceNum = 15;
                    break;
            }
        }
        fences.SetTile(new(x - isFence.GetLength(0) / 2, y - isFence.GetLength(1) / 2), rightFenceNum == -1 ? null : fenceTiles[rightFenceNum]);
    }

    private void SetTilesToRandom()
    {
        foreach (Vector3Int position in ground.cellBounds.allPositionsWithin)
        {
            ground.SetTile(position, GetRandomTile());
            TryToSpawnATree(position);
        }
    }

    private TileBase GetRandomTile()
    {
        int totalChance = 0;
        foreach (int chance in tileWeights) totalChance += chance;
        int randomValue = UnityEngine.Random.Range(0, totalChance);
        int chanceUsed = 0;
        for (int i = 0; i < groundTiles.Length; i++)
        {
            chanceUsed += tileWeights[i];
            if (randomValue < chanceUsed) return groundTiles[i];
        }
        return null;
    }

    private void TryToSpawnATree(Vector3Int position)
    {
        if (fences.GetTile(position) != null) return;
        int chance = 2 * (Math.Abs(position.x) + Math.Abs(position.y));
        if (chance == 0) return;
        if (chance > 2 * maxTreeChance) chance = 90;
        int random = UnityEngine.Random.Range(0, 100 - chance);
        if (random == 0)
        {
            trees.SetTile(position, treeTiles[UnityEngine.Random.Range(0, 2)]);
        }
    }
}
