// HPBarsManager.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class HPBarsManager : MonoBehaviour
{
    [Header("UI 側の Controller (順番に P1, P2)")]
    [SerializeField] HPBarController[] controllers; // サイズ 2

    PlayerInputManager pim;

    void Awake()
    {
        pim = FindObjectOfType<PlayerInputManager>();
        if (pim == null)
        {
            Debug.LogError("PlayerInputManager がシーンにありません！");
            enabled = false;
            return;
        }
        pim.onPlayerJoined += OnPlayerJoined;
    }

    void OnDestroy()
    {
        if (pim != null)
            pim.onPlayerJoined -= OnPlayerJoined;
    }

    void OnPlayerJoined(PlayerInput pi)
    {
        var status = pi.GetComponent<PlayerStatus>();
        int idx = pi.playerIndex; // 0 = P1, 1 = P2

        if (status == null)
        {
            Debug.LogWarning($"PlayerStatus が見つかりません: {pi.gameObject.name}");
            return;
        }
        if (idx < 0 || idx >= controllers.Length)
        {
            Debug.LogWarning($"HPBarController の設定外のインデックス: {idx}");
            return;
        }

        // View は Controller の子コンポーネントから自動取得
        var view = controllers[idx].GetComponent<HPBarView>();
        controllers[idx].Initialize(status, view);

        Debug.Log($"Player{idx + 1} を HPBarController[{idx}] に紐づけました。");
    }
}
