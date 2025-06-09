// HPBarView.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;       // TextMeshPro を使う場合
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class HPBarView : MonoBehaviour
{
    public Color OriginalColor { get; set; }

    [SerializeField] float tweenDuration = 0.5f;
    [Header("Icon と数字表示 (任意)")]
    [SerializeField] Image iconImage;     // アイコン Image
    [SerializeField] TMP_Text hpText;      // TextMeshProUGUI も可
    // もし普通の UI.Text なら: using UnityEngine.UI; [SerializeField] Text hpText;

    Image fillImage;
    Color originalColor;

    void Awake()
    {
        fillImage = GetComponent<Image>();
        originalColor = fillImage.color;
    }

    /// <summary>
    /// 数値とアイコン・バーを更新する
    /// </summary>
    public void UpdateHPBar(float normalized, int currentHP, int maxHP)
    {
        // 1. 数字
        if (hpText != null)
            hpText.text = $"{currentHP} / {maxHP}";
        // 2. アイコンは特別に変えないなら省略
        // 3. バー（アニメーション）
        fillImage.DOKill();
        fillImage.DOFillAmount(Mathf.Clamp01(normalized), tweenDuration);
    }

    /// <summary>
    /// アニメなしで即時セット
    /// </summary>
    public void SetFillImmediate(float normalized, int currentHP, int maxHP)
    {
        hpText.text = $"{currentHP} / {maxHP}";
        fillImage.DOKill();
        fillImage.fillAmount = normalized;
    }

    /// <summary>
    /// バーの色を即時変える
    /// </summary>
    public void SetColor(Color c)
    {
        fillImage.color = c;
    }

    /// <summary>
    /// 瞬間点滅アニメーション
    /// </summary>
    public Sequence DoFlash(Color flashColor, float flashDuration = 0.1f)
    {
        // 最新のバー色を取得
        Color returnColor = OriginalColor;
        return DOTween.Sequence()
            .Append(fillImage.DOColor(flashColor, flashDuration))
            .Append(fillImage.DOColor(returnColor, flashDuration));
    }
}
