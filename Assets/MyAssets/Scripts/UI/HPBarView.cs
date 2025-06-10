// HPBarView.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;       // TextMeshPro ���g���ꍇ
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class HPBarView : MonoBehaviour
{
    public Color OriginalColor { get; set; }

    [SerializeField] float tweenDuration = 0.5f;
    [Header("Icon �Ɛ����\�� (�C��)")]
    [SerializeField] Image iconImage;     // �A�C�R�� Image
    [SerializeField] TMP_Text hpText;      // TextMeshProUGUI ����
    // �������ʂ� UI.Text �Ȃ�: using UnityEngine.UI; [SerializeField] Text hpText;

    Image fillImage;
    Color originalColor;

    void Awake()
    {
        fillImage = GetComponent<Image>();
        originalColor = fillImage.color;
    }

    /// <summary>
    /// ���l�ƃA�C�R���E�o�[���X�V����
    /// </summary>
    public void UpdateHPBar(float normalized, int currentHP, int maxHP)
    {
        // 1. ����
        if (hpText != null)
            hpText.text = $"{currentHP} / {maxHP}";
        // 2. �A�C�R���͓��ʂɕς��Ȃ��Ȃ�ȗ�
        // 3. �o�[�i�A�j���[�V�����j
        fillImage.DOKill();
        fillImage.DOFillAmount(Mathf.Clamp01(normalized), tweenDuration);
    }

    /// <summary>
    /// �A�j���Ȃ��ő����Z�b�g
    /// </summary>
    public void SetFillImmediate(float normalized, int currentHP, int maxHP)
    {
        hpText.text = $"{currentHP} / {maxHP}";
        fillImage.DOKill();
        fillImage.fillAmount = normalized;
    }

    /// <summary>
    /// �o�[�̐F�𑦎��ς���
    /// </summary>
    public void SetColor(Color c)
    {
        fillImage.color = c;
    }

    /// <summary>
    /// �u�ԓ_�ŃA�j���[�V����
    /// </summary>
    public Sequence DoFlash(Color flashColor, float flashDuration = 0.1f)
    {
        // �ŐV�̃o�[�F���擾
        Color returnColor = OriginalColor;
        return DOTween.Sequence()
            .Append(fillImage.DOColor(flashColor, flashDuration))
            .Append(fillImage.DOColor(returnColor, flashDuration));
    }
}
