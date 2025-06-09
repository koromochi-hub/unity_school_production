using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshPro を使うなら
public class TrapUIController : MonoBehaviour
{
    [Header("どのプレイヤーのトラップを監視するか")]
    public PlayerTrapController playerTrap;

    [Header("UI の参照")]
    public Image iconImage;
    public TMP_Text countText;  // UI.Text を使うなら Text 型

    [Header("トラップ毎のアイコン一覧 (PlayerTrapController.trapPrefabs と同順)")]
    public Sprite[] trapIcons;

    void Start()
    {
        // イベント購読
        playerTrap.OnTrapSwitched += HandleTrapSwitched;
        // 初期表示
        // Awake で既に NotifyTrapSwitched しているならここでもOK
        // playerTrap.OnTrapSwitched が最初に飛んでくる
    }

    void OnDestroy()
    {
        playerTrap.OnTrapSwitched -= HandleTrapSwitched;
    }

    // イベントハンドラ
    void HandleTrapSwitched(int index, int curCount, int maxCount)
    {
        // 1. アイコン差し替え
        if (index >= 0 && index < trapIcons.Length)
            iconImage.sprite = trapIcons[index];

        // 2. 数字更新
        countText.text = $"{curCount} / {maxCount}";
    }
}
