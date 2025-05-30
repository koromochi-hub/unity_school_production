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

    public void PlaceTrap(Vector2Int gridPos, GameObject trapPrefab, PlayerStatus owner, PlayerTrapController trapController, int trapTypeIndex)
    {
        if(CanSetTrap(gridPos))
        {
            // 0.5ずらしてるけど、消してOK
            Vector3 worldPos = new Vector3(gridPos.x + 0.5f, 0.5f, gridPos.y + 0.5f);
            GameObject trap = Instantiate(trapPrefab, worldPos, Quaternion.identity);


            // プレイヤーごとに判定してレイヤーを設定
            int trapLayer = -1;

            if (owner.playerId == 1)
                trapLayer = LayerMask.NameToLayer("Trap_P1");
            else if (owner.playerId == 2)
                trapLayer = LayerMask.NameToLayer("Trap_P2");

            // レイヤー(0 ～ 31)が未設定の場合
            if (trapLayer != -1)
                SetLayerRecursively(trap, trapLayer);

            TrapBase trapbase = trap.GetComponent<TrapBase>();
            trapbase.Initialize(gridPos, owner, trapController, trapTypeIndex);

            placedTraps[gridPos] = trap;
        }
    }

    // トラップの子オブジェクトにもレイヤー追加
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
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