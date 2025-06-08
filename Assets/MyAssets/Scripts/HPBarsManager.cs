// HPBarsManager.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class HPBarsManager : MonoBehaviour
{
    [Header("UI ���� Controller (���Ԃ� P1, P2)")]
    [SerializeField] HPBarController[] controllers; // �T�C�Y 2

    PlayerInputManager pim;

    void Awake()
    {
        pim = FindObjectOfType<PlayerInputManager>();
        if (pim == null)
        {
            Debug.LogError("PlayerInputManager ���V�[���ɂ���܂���I");
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
            Debug.LogWarning($"PlayerStatus ��������܂���: {pi.gameObject.name}");
            return;
        }
        if (idx < 0 || idx >= controllers.Length)
        {
            Debug.LogWarning($"HPBarController �̐ݒ�O�̃C���f�b�N�X: {idx}");
            return;
        }

        // View �� Controller �̎q�R���|�[�l���g���玩���擾
        var view = controllers[idx].GetComponent<HPBarView>();
        controllers[idx].Initialize(status, view);

        Debug.Log($"Player{idx + 1} �� HPBarController[{idx}] �ɕR�Â��܂����B");
    }
}
