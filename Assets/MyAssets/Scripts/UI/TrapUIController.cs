// TrapUIController.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrapUIController : MonoBehaviour
{
    [Header("UI の参照 (子オブジェクトなど)")]
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text countText;

    [Header("トラップ毎のアイコン一覧(TrapPrefabs と同順)")]
    [SerializeField] Sprite[] trapIcons;

    // 監視する PlayerTrapController
    private PlayerTrapController playerTrap;

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
        // アイコン
        if (index >= 0 && index < trapIcons.Length)
            iconImage.sprite = trapIcons[index];

        // カウント表示
        countText.text = $"{curCount} / {maxCount}";

        countText.color = curCount <= 0
            ? Color.red
            : Color.white;
    }
}
