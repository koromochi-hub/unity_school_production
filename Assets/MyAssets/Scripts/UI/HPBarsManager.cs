// HPBarsManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HPBarsManager : MonoBehaviour
{
    [Header("UI 側の Controller (順番に P1, P2)")]
    [SerializeField] HPBarController[] controllers; // サイズ 2

    PlayerInputManager pim;

    void Awake()
    {
        pim = FindFirstObjectByType<PlayerInputManager>();
        if (pim == null)
        {
            Debug.LogError("PlayerInputManager が見つかりません");
            enabled = false;
            return;
        }
        pim.onPlayerJoined += pi => StartCoroutine(BindHPBarNextFrame(pi));
    }
    void OnDestroy()
    {
        if (pim != null)
            pim.onPlayerJoined -= pi => StartCoroutine(BindHPBarNextFrame(pi));
    }


    IEnumerator BindHPBarNextFrame(PlayerInput pi)
    {
        yield return null;
        var status = pi.GetComponent<PlayerStatus>();
        int idx = pi.playerIndex;
        if (status == null || idx < 0 || idx >= controllers.Length)
        {
            Debug.LogWarning($"HPBar 初期化失敗: status={(status == null)}, idx={idx}");
            yield break;
        }
        var view = controllers[idx].GetComponent<HPBarView>();
        controllers[idx].Initialize(status, view);
    }
}
