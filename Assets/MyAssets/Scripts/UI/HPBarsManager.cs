// HPBarsManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HPBarsManager : MonoBehaviour
{
    [Header("UI ���� Controller (���Ԃ� P1, P2)")]
    [SerializeField] HPBarController[] controllers; // �T�C�Y 2

    PlayerInputManager pim;

    void Awake()
    {
        pim = FindFirstObjectByType<PlayerInputManager>();
        if (pim == null)
        {
            Debug.LogError("PlayerInputManager ��������܂���");
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
            Debug.LogWarning($"HPBar ���������s: status={(status == null)}, idx={idx}");
            yield break;
        }
        var view = controllers[idx].GetComponent<HPBarView>();
        controllers[idx].Initialize(status, view);
    }
}
