// HPBarsManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HPBarsManager : MonoBehaviour
{
    [Header("UI ë§ÇÃ Controller (èáî‘Ç… P1, P2)")]
    [SerializeField] HPBarController[] controllers; // ÉTÉCÉY 2

    PlayerInputManager pim;

    void Awake()
    {
        pim = FindFirstObjectByType<PlayerInputManager>();
        if (pim == null)
        {
            Debug.LogError("PlayerInputManager Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒ");
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
            Debug.LogWarning($"HPBar èâä˙âªé∏îs: status={(status == null)}, idx={idx}");
            yield break;
        }
        var view = controllers[idx].GetComponent<HPBarView>();
        controllers[idx].Initialize(status, view);
    }
}
