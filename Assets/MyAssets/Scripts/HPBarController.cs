// HPBarController.cs
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    PlayerStatus model;
    HPBarView view;
    float lastHP;  // 前回 HP (点滅判定用)

    /// <summary>
    /// Manager から呼ぶ初期化
    /// </summary>
    public void Initialize(PlayerStatus model, HPBarView view)
    {
        this.model = model;
        this.view = view;

        // ① 初期表示（即時反映＋数値表示）
        float initRatio = (float)model.CurrentHP / model.MaxHP;
        view.SetFillImmediate(initRatio, model.CurrentHP, model.MaxHP);

        // ② 初期色を確実に適用
        ApplyColor(initRatio);

        // ③ モデルの変化を購読
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

        // 1. 数値＆バー更新
        view.UpdateHPBar(ratio, current, max);

        // 2. カラー変更
        ApplyColor(ratio);

        // 3. 点滅アニメ（ダメージ時のみ）
        if (current < lastHP)
            view.DoFlash(Color.red);

        lastHP = current;
    }

    /// <summary>
    /// HP比率に応じて色を変える
    /// </summary>
    void ApplyColor(float ratio)
    {
        Color newColor;
        if (ratio < 0.3f) newColor = Color.red;
        else if (ratio < 0.7f) newColor = Color.yellow;
        else newColor = Color.green;

        view.SetColor(newColor);
        // 点滅後にこの色へ戻すためにキャッシュ
        view.OriginalColor = newColor;
    }

}
