// HPBarController.cs
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    PlayerStatus model;
    HPBarView view;

    /// <summary>
    /// Manager ‚©‚çŒÄ‚Î‚ê‚Ä‰Šú‰»BModel ‚Æ View ‚ğ•R•t‚¯‚éB
    /// </summary>
    public void Initialize(PlayerStatus model, HPBarView view)
    {
        this.model = model;
        this.view  = view;

        // Model ‚Ì•Ï‰»‚ğw“Ç
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
