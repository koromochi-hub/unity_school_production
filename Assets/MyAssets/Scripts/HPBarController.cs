// HPBarController.cs
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    PlayerStatus model;
    HPBarView view;

    /// <summary>
    /// Manager ����Ă΂�ď������BModel �� View ��R�t����B
    /// </summary>
    public void Initialize(PlayerStatus model, HPBarView view)
    {
        this.model = model;
        this.view  = view;

        // Model �̕ω����w��
        model.OnHPChanged += HandleHPChanged;
    }

    void OnDestroy()
    {
        if (model != null)
            model.OnHPChanged -= HandleHPChanged;
    }

    void HandleHPChanged(int current, int max)
    {
        float ratio = (float)current / max;
        view.UpdateHPBar(ratio);
    }
}
