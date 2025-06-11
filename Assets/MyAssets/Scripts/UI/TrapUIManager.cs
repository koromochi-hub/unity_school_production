using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TrapUIManager : MonoBehaviour
{
    [Header("UI 側の TrapUIController (順番に P1, P2, …)")]
    [SerializeField] TrapUIController[] controllers;

    private PlayerInputManager pim;

    void Awake()
    {
        pim = FindFirstObjectByType<PlayerInputManager>();
        if (pim == null)
        {
            Debug.LogError("PlayerInputManager が見つかりません");
            enabled = false;
            return;
        }

        // 新規参加時のバインド
        pim.onPlayerJoined += OnPlayerJoined;
    }

    void Start()
    {
        // すでにスポーン済みのプレイヤーもまとめてバインド
        foreach (var pi in PlayerInput.all)
        {
            // PlayerInputManager 経由で生成されたものだけに絞る場合は
            // if (pi.manager == pim) ... としてください
            StartCoroutine(BindTrapUINextFrame(pi));
        }
    }

    void OnDestroy()
    {
        if (pim != null)
            pim.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput pi)
    {
        StartCoroutine(BindTrapUINextFrame(pi));
    }

    private IEnumerator BindTrapUINextFrame(PlayerInput pi)
    {
        yield return null;  // 一フレーム待つ

        var ptc = pi.GetComponent<PlayerTrapController>();
        int idx = pi.playerIndex;

        if (ptc == null || idx < 0 || idx >= controllers.Length)
        {
            Debug.LogWarning($"TrapUI の初期化失敗: ptc={(ptc == null)}, idx={idx}");
            yield break;
        }

        controllers[idx].Initialize(ptc);
    }
}
