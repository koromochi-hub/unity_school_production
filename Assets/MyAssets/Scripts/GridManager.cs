using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;

    // É}ÉXÇ…ê›íuÇ≥ÇÍÇΩ„©ÇÃèÓïÒÇï€éù
    private Dictionary<Vector2Int, GameObject> trapMap = new Dictionary<Vector2Int, GameObject>();

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / cellSize);
        int z = Mathf.FloorToInt(worldPos.z / cellSize);
        return new Vector2Int(x, z);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        float x = gridPos.x * cellSize + cellSize / 2f;
        float z = gridPos.y * cellSize + cellSize / 2f;
        return new Vector3(x, 0.5f, z); // yÇÕçÇÇ≥
    }

    public bool CanPlaceTrap(Vector2Int gridPos)
    {
        return !trapMap.ContainsKey(gridPos);
    }

    public void PlaceTrap(Vector2Int gridPos, GameObject trapPrefab)
    {
        if (!CanPlaceTrap(gridPos)) return;

        Vector3 trapWorldPos = GridToWorld(gridPos);
        GameObject trapInstance = Instantiate(trapPrefab, trapWorldPos, Quaternion.identity);
        trapMap[gridPos] = trapInstance;
    }
}