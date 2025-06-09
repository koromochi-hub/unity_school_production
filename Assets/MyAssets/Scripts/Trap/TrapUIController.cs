using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshPro ���g���Ȃ�
public class TrapUIController : MonoBehaviour
{
    [Header("�ǂ̃v���C���[�̃g���b�v���Ď����邩")]
    public PlayerTrapController playerTrap;

    [Header("UI �̎Q��")]
    public Image iconImage;
    public TMP_Text countText;  // UI.Text ���g���Ȃ� Text �^

    [Header("�g���b�v���̃A�C�R���ꗗ (PlayerTrapController.trapPrefabs �Ɠ���)")]
    public Sprite[] trapIcons;

    void Start()
    {
        // �C�x���g�w��
        playerTrap.OnTrapSwitched += HandleTrapSwitched;
        // �����\��
        // Awake �Ŋ��� NotifyTrapSwitched ���Ă���Ȃ炱���ł�OK
        // playerTrap.OnTrapSwitched ���ŏ��ɔ��ł���
    }

    void OnDestroy()
    {
        playerTrap.OnTrapSwitched -= HandleTrapSwitched;
    }

    // �C�x���g�n���h��
    void HandleTrapSwitched(int index, int curCount, int maxCount)
    {
        // 1. �A�C�R�������ւ�
        if (index >= 0 && index < trapIcons.Length)
            iconImage.sprite = trapIcons[index];

        // 2. �����X�V
        countText.text = $"{curCount} / {maxCount}";
    }
}
