using System.Collections.Generic;
using UnityEngine;

public class PlayerSearch : MonoBehaviour
{
    [Header("索敵設定")]
    [SerializeField] private GameObject highlightPrefab;     // 索敵範囲表示用プレハブ
    [SerializeField] private GameObject trapMarkerPrefab;    // マーカー用プレハブ
    [SerializeField] private int searchRange = 2;            // 周囲2マス（5×5範囲）

    // ── 内部管理用リスト／辞書 ──
    private List<GameObject> highlights = new List<GameObject>();
    private HashSet<Vector2Int> discoveredTraps = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, GameObject> trapMarkers = new Dictionary<Vector2Int, GameObject>();

    private PlayerStatus playerStatus;
    private bool isSearching = false;
    private Vector2Int lastPlayerGridPos;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        lastPlayerGridPos = GridManager.Instance.WorldToGrid(transform.position);

        // 1) GridManager の OnTrapRemoved イベントを購読して、
        //    トラップ削除時にマーカーも自動で消すようにする
        GridManager.Instance.OnTrapRemoved += HandleTrapRemoved;
    }

    private void OnDestroy()
    {
        // 2) イベントを解除しないと、オブジェクトが破棄されたあとに
        //    このメソッドが呼ばれ続ける可能性があるため必ず解除する
        if (GridManager.Instance != null)
        {
            GridManager.Instance.OnTrapRemoved -= HandleTrapRemoved;
        }
    }

    private void Update()
    {
        if (!isSearching) return;

        Vector2Int currentGrid = GridManager.Instance.WorldToGrid(transform.position);
        if (currentGrid != lastPlayerGridPos)
        {
            // プレイヤーがマス移動したらハイライトと相手トラップの確認を更新
            RefreshSearchVisuals(currentGrid);
            lastPlayerGridPos = currentGrid;
        }
    }

    /// <summary>
    /// 索敵を開始する（外部から呼び出す）
    /// </summary>
    public void BeginSearch()
    {
        if (isSearching) return;
        isSearching = true;

        Vector2Int playerGridPos = GridManager.Instance.WorldToGrid(transform.position);
        RefreshSearchVisuals(playerGridPos);
        lastPlayerGridPos = playerGridPos;
    }

    /// <summary>
    /// 索敵を終了する（外部から呼び出す）
    /// ハイライトだけ消し、マーカーは残す
    /// </summary>
    public void EndSearch()
    {
        if (!isSearching) return;
        isSearching = false;

        // ハイライトだけを Destroy
        foreach (var h in highlights)
        {
            if (h != null) Destroy(h);
        }
        highlights.Clear();

        // ※ trapMarkers（マーカー）は残し、discoveredTraps もクリアしないので
        //    既に発見したものは次回以降も再表示されない
    }

    /// <summary>
    /// プレイヤーのグリッド位置に合わせて
    /// ハイライトを再生成し、相手トラップをチェックしてマーカーを表示する
    /// </summary>
    private void RefreshSearchVisuals(Vector2Int playerGridPos)
    {
        // 1) 既存のハイライトをすべて削除（マーカーは残す）
        foreach (var h in highlights)
        {
            if (h != null) Destroy(h);
        }
        highlights.Clear();

        // 2) -searchRange 〜 +searchRange をループしてハイライトを配置しながらトラップチェック
        for (int dx = -searchRange; dx <= searchRange; dx++)
        {
            for (int dy = -searchRange; dy <= searchRange; dy++)
            {
                Vector2Int checkPos = playerGridPos + new Vector2Int(dx, dy);

                // (a) ハイライトを置く
                Vector3 highlightWorld = new Vector3(checkPos.x + 0.5f, 0.01f, checkPos.y + 0.5f);
                var highlight = Instantiate(
                    highlightPrefab,
                    highlightWorld,
                    Quaternion.Euler(90, 0, 0)
                );
                highlights.Add(highlight);

                // (b) そのマスにトラップがあるかチェック
                if (GridManager.Instance.TryGetTrapData(checkPos, out GameObject trapObj))
                {
                    var tb = trapObj.GetComponent<TrapBase>();
                    if (tb == null) continue;

                    // 自分のトラップならスキップ
                    if (tb.Owner.playerId == playerStatus.playerId) continue;

                    // 相手のトラップなら、まだ見つけていなければマーカーを生成
                    if (!discoveredTraps.Contains(checkPos))
                    {
                        discoveredTraps.Add(checkPos);
                        Vector3 markerWorld = new Vector3(checkPos.x + 0.5f, 0.5f, checkPos.y + 0.5f);
                        var marker = Instantiate(trapMarkerPrefab, markerWorld, Quaternion.identity);
                        trapMarkers[checkPos] = marker;
                    }
                }
            }
        }
    }

    /// <summary>
    /// GridManager.OnTrapRemoved イベントから呼ばれるハンドラ。
    /// 引数 gridPos に対応するマーカーが残っていれば Destroy & 辞書から除去する。
    /// </summary>
    private void HandleTrapRemoved(Vector2Int gridPos)
    {
        // ① discoveredTraps からも除去しておく（必要に応じて）
        if (discoveredTraps.Contains(gridPos))
        {
            discoveredTraps.Remove(gridPos);
        }

        // ② trapMarkers 辞書に該当キーがあれば Destroy & Remove
        if (trapMarkers.TryGetValue(gridPos, out GameObject existingMarker))
        {
            if (existingMarker != null) Destroy(existingMarker);
            trapMarkers.Remove(gridPos);
        }
    }
}
