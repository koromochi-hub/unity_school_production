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
        pim = FindFirstObjectByType<PlayerInputManager>();
        if (pim == null)
        {
            Debug.LogError("PlayerInputManager ��������܂���");
            enabled = false;
            return;
        }

        // �V�K�Q�����̃o�C���h
        pim.onPlayerJoined += OnPlayerJoined;
    }

    void Start()
    {
        // ���łɃX�|�[���ς݂̃v���C���[���܂Ƃ߂ăo�C���h
        foreach (var pi in PlayerInput.all)
        {
            // PlayerInputManager �o�R�Ő������ꂽ���̂����ɍi��ꍇ��
            // if (pi.manager == pim) ... �Ƃ��Ă�������
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
        yield return null;  // ��t���[���҂�

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
