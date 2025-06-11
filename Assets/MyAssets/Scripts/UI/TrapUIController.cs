// TrapUIController.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrapUIController : MonoBehaviour
{
    [Header("UI �̎Q�� (�q�I�u�W�F�N�g�Ȃ�)")]
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text countText;

    [Header("�g���b�v���̃A�C�R���ꗗ(TrapPrefabs �Ɠ���)")]
    [SerializeField] Sprite[] trapIcons;

    // �Ď����� PlayerTrapController
    private PlayerTrapController playerTrap;

    /// <summary>
    /// Manager ����Ă�ŕR�Â���
    /// </summary>
    public void Initialize(PlayerTrapController ptc)
    {
        // �����ȑO�ɕR�Â��Ă��������
        if (playerTrap != null)
            playerTrap.OnTrapSwitched -= HandleTrapSwitched;

        playerTrap = ptc;
        // �C�x���g�o�^
        playerTrap.OnTrapSwitched += HandleTrapSwitched;
        // �����\��
        // �����I�ɍ�������x�n���h�����Ă�:
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
        // �A�C�R��
        if (index >= 0 && index < trapIcons.Length)
            iconImage.sprite = trapIcons[index];

        // �J�E���g�\��
        countText.text = $"{curCount} / {maxCount}";

        countText.color = curCount <= 0
            ? Color.red
            : Color.white;
    }
}
