// TrapUIManager.cs
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TrapUIManager : MonoBehaviour
{
    [Header("UI ���� TrapUIController (���Ԃ� P1, P2, �c)")]
    [SerializeField] TrapUIController[] controllers;

    private PlayerInputManager pim;

    void Awake()
    {
        pim = FindObjectOfType<PlayerInputManager>();
        if (pim == null)
        {
            Debug.LogError("PlayerInputManager ��������܂���");
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
        // �t���[�������肵�Ă���o�C���h
        StartCoroutine(BindTrapUINextFrame(pi));
    }

    private IEnumerator BindTrapUINextFrame(PlayerInput pi)
    {
        yield return null;

        var ptc = pi.GetComponent<PlayerTrapController>();
        int idx = pi.playerIndex;

        if (ptc == null || idx < 0 || idx >= controllers.Length)
        {
            Debug.LogWarning($"TrapUI �̏��������s: ptc={(ptc == null)}, idx={idx}");
            yield break;
        }

        controllers[idx].Initialize(ptc);
    }
}
