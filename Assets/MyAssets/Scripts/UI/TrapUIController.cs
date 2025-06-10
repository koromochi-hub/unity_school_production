// TrapUIController.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;


public class TrapUIController : MonoBehaviour
{
    [Header("UI の参照 (子オブジェクトなど)")]
    [SerializeField] private Transform iconContainer;
    [SerializeField] TMP_Text countText;

    [Header("トラップ毎のアイコン一覧(TrapPrefabs と同順)")]
    [SerializeField] private GameObject[] trapIconPrefabs;

    // 監視する PlayerTrapController
    private PlayerTrapController playerTrap;
    private GameObject currentIconModel;

    /// <summary>
    /// Manager から呼んで紐づける
    /// </summary>
    public void Initialize(PlayerTrapController ptc)
    {
        // もし以前に紐づいていたら解除
        if (playerTrap != null)
            playerTrap.OnTrapSwitched -= HandleTrapSwitched;

        playerTrap = ptc;
        // イベント登録
        playerTrap.OnTrapSwitched += HandleTrapSwitched;
        // 初期表示
        // 明示的に今すぐ一度ハンドラを呼ぶ:
        HandleTrapSwitched(playerTrap.CurrentIndex,
                           playerTrap.GetCurrentCount(),
                           playerTrap.GetMaxCount());
    }

    void OnDestroy()
    {
        if (playerTrap != null)
            playerTrap.OnTrapSwitched -= HandleTrapSwitched;
    }

    private void HandleTrapSwitched(int index, int curCount, int maxCount)
    {
        // (1) まず過去のモデルを消す
        if (currentIconModel != null)
            Destroy(currentIconModel);

        // (2) 新しいモデルをアイコンコンテナに生成
        if (index >= 0 && index < trapIconPrefabs.Length)
        {
            currentIconModel = Instantiate(
                trapIconPrefabs[index],
                iconContainer,
                worldPositionStays: false
            );

            // ① 親と同じレイヤーに切り替え
            SetLayerRecursively(currentIconModel.transform, iconContainer.gameObject.layer);

            // ② ソートグループを追加して UI ソートレイヤー内で描画
            var sg = currentIconModel.AddComponent<SortingGroup>();
            sg.sortingLayerID = SortingLayer.NameToID("UI");
            sg.sortingOrder = 1;

            // 必要に応じてスケール／回転を微調整
            currentIconModel.transform.localPosition = Vector3.zero;
        }

        // カウント表示
        countText.text = $"{curCount} / {maxCount}";

        countText.color = curCount <= 0
            ? Color.red
            : Color.white;
    }

    /// <summary>
    /// transform 以下を一括で layer を揃えるヘルパー
    /// </summary>
    void SetLayerRecursively(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        foreach (Transform c in t)
            SetLayerRecursively(c, layer);
    }



}
