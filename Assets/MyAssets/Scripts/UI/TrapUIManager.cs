// TrapUIManager.cs
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
        pim = FindObjectOfType<PlayerInputManager>();
        if (pim == null)
        {
            Debug.LogError("PlayerInputManager が見つかりません");
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

    private void OnPlayerJoined(PlayerInput pi)
    {
        // フレームが安定してからバインド
        StartCoroutine(BindTrapUINextFrame(pi));
    }

    private IEnumerator BindTrapUINextFrame(PlayerInput pi)
    {
        yield return null;

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
