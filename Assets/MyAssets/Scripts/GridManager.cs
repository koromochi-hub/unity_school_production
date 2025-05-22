using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private Dictionary<Vector2Int, GameObject> placedTraps = new Dictionary<Vector2Int, GameObject>();

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.z);
        return new Vector2Int(x, y);
    }

    public bool CanSetTrap(Vector2Int gridPos)
    {
        return !placedTraps.ContainsKey(gridPos);
    }

    public void PlaceTrap(Vector2Int gridPos, GameObject trapPrefab)
    {
        if (CanSetTrap(gridPos))
        {
            Vector3 worldPos = new Vector3(gridPos.x + 0.5f, 0.5f, gridPos.y + 0.5f);
            GameObject trap = Instantiate(trapPrefab, worldPos, Quaternion.identity);
            TrapBase trapBase = trap.GetComponent<TrapBase>();
            if (trapBase != null)
            {
                trapBase.SetGridPosition(gridPos);
            }
            placedTraps[gridPos] = trap;
        }
    }

    public void ClearTrap(Vector2Int gridPos)
    {
        if (placedTraps.ContainsKey(gridPos))
        {
            placedTraps.Remove(gridPos);
        }
    }
}