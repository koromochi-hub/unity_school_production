// HPBarController.cs
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    PlayerStatus model;
    HPBarView view;
    float lastHP;  // �O�� HP (�_�Ŕ���p)

    /// <summary>
    /// Manager ����Ăԏ�����
    /// </summary>
    public void Initialize(PlayerStatus model, HPBarView view)
    {
        this.model = model;
        this.view = view;

        // �@ �����\���i�������f�{���l�\���j
        float initRatio = (float)model.CurrentHP / model.MaxHP;
        view.SetFillImmediate(initRatio, model.CurrentHP, model.MaxHP);

        // �A �����F���m���ɓK�p
        ApplyColor(initRatio);

        // �B ���f���̕ω����w��
        model.OnHPChanged += HandleHPChanged;
        lastHP = model.CurrentHP;
    }

    void OnDestroy()
    {
        if (model != null)
            model.OnHPChanged -= HandleHPChanged;
    }

    void HandleHPChanged(int current, int max)
    {
        float ratio = (float)current / max;

        // 1. ���l���o�[�X�V
        view.UpdateHPBar(ratio, current, max);

        // 2. �J���[�ύX
        ApplyColor(ratio);

        // 3. �_�ŃA�j���i�_���[�W���̂݁j
        if (current < lastHP)
            view.DoFlash(Color.red);

        lastHP = current;
    }

    /// <summary>
    /// HP�䗦�ɉ����ĐF��ς���
    /// </summary>
    void ApplyColor(float ratio)
    {
        Color newColor;
        if (ratio < 0.3f) newColor = Color.red;
        else if (ratio < 0.7f) newColor = Color.yellow;
        else newColor = Color.green;

        view.SetColor(newColor);
        // �_�Ō�ɂ��̐F�֖߂����߂ɃL���b�V��
        view.OriginalColor = newColor;
    }

}
