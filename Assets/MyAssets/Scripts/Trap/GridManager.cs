using System;
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

    // トラップ削除時に通知するイベントを宣言
    public event Action<Vector2Int> OnTrapRemoved;

    // 設置済みトラップを管理する辞書(Group) ⇒ key=グリッド座標、value=Trap本体(GameObject)
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
            float worldX = gridPos.x + 0.5f;
            float worldZ = gridPos.y + 0.5f;

            float worldY = owner.transform.position.y + 0.5f;

            Vector3 worldPos = new Vector3(worldX, worldY, worldZ);
            GameObject trap = Instantiate(trapPrefab, worldPos, Quaternion.identity);


            // プレイヤーごとに判定してレイヤーを設定
            int trapLayer = -1;

            if (owner.playerId == 0)
                trapLayer = LayerMask.NameToLayer("Trap_P1");
            else if (owner.playerId == 1)
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

    /// <summary>
    /// 指定座標にあるトラップを削除する。
    /// 内部の placedTraps から除外し、GameObject を Destroy し、さらにイベントを発火する。
    /// </summary>
    public void ClearTrap(Vector2Int gridPos)
    {
        if (!placedTraps.ContainsKey(gridPos)) return;

        // ① Scene 上に置かれているトラップ本体を Destroy
        GameObject trap = placedTraps[gridPos];
        if (trap != null)
        {
            Destroy(trap);
        }

        // ② 辞書からもキーを除外
        placedTraps.Remove(gridPos);

        // ③ トラップ削除イベントを発火（購読しているスクリプトに通知）
        OnTrapRemoved?.Invoke(gridPos);
    }

    /// <summary>
    /// トラップが存在する場合、そのトラップ（ GameObject ）を trap という変数に代入し、 true を返す。
    /// </summary>
    public bool TryGetTrapData(Vector2Int gridPos, out GameObject trap)
    {
        return placedTraps.TryGetValue(gridPos, out trap);
    }
}