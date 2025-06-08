// HPBarView.cs
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class HPBarView : MonoBehaviour
{
    [SerializeField] float tweenDuration = 0.5f;
    Image fillImage;

    void Awake()
    {
        fillImage = GetComponent<Image>();
        // Image Type = Filled / Horizontal に設定済みであること
    }

    /// <summary>
    /// コントローラから呼ばれて、バーをアニメーション更新する
    /// </summary>
    public void UpdateHPBar(float normalized)
    {
        fillImage.DOFillAmount(Mathf.Clamp01(normalized), tweenDuration);
    }
}
