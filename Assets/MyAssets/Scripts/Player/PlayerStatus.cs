using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(Animator))]
public class PlayerStatus : MonoBehaviour
{
    public int playerId;

    [Header("HP設定")]
    public int maxHP = 100;
    private int currentHP;

    [Header("ノックバック設定")]
    [Tooltip("横方向のノックバック強度")]
    [SerializeField] private float knockbackForce = 8f;

    [Tooltip("上方向のノックバック強度")]
    [SerializeField] private float knockbackUpForce = 5f;

    [Tooltip("ノックバックが有効な時間 (秒)")]
    [SerializeField] private float knockbackDuration = 1.0f;

    // 内部キャッシュ
    private Rigidbody rb;
    private Animator animator;
    private PlayerMove playerMove;

    // ノックバック用
    private bool isKnocked = false;
    private float knockbackTimer = 0f;
    private Vector3 knockbackVelocity = Vector3.zero;

    private void Awake()
    {
        // 必須コンポーネントをキャッシュ
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();

        if (rb == null)
            Debug.LogError("PlayerStatus requires a Rigidbody.");
        if (animator == null)
            Debug.LogError("PlayerStatus requires an Animator.");
        if (playerMove == null)
            Debug.LogError("PlayerStatus requires a PlayerMove.");

        currentHP = maxHP;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        // ノックバック中であれば、velocity を固定し続ける
        if (isKnocked)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = knockbackVelocity;

            if (knockbackTimer <= 0f)
            {
                EndKnockback();
            }
        }
    }

    /// <summary>
    /// ダメージを受けたときに呼び出す
    /// damage: 与えるダメージ量 (整数)
    /// knockbackDirection: XZ平面上の方向ベクトル (Y=0推奨)
    /// </summary>
    public void TakeDamage(int damage, Vector3 knockbackDirection)
    {
        // HP 減算
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        Debug.Log($"{gameObject.name} は {damage} ダメージを受けた。残りHP: {currentHP}");

        // ノックバックを開始
        StartKnockback(knockbackDirection);
    }

    /// <summary>
    /// ノックバックを開始する内部メソッド
    /// </summary>
    private void StartKnockback(Vector3 direction)
    {
        if (isKnocked) return; // すでにノックバック中なら無視

        isKnocked = true;
        knockbackTimer = knockbackDuration;

        // プレイヤー操作を無効にする
        playerMove.enabled = false;

        // 横方向の速度
        Vector3 horizVel = direction.normalized * knockbackForce;
        // 上方向の速度
        Vector3 upVel = Vector3.up * knockbackUpForce;
        // 合成ベクトル
        knockbackVelocity = horizVel + upVel;

        // Rigidbody に初速を設定
        rb.linearVelocity = knockbackVelocity;

        // ダウンアニメーションを再生 (DownTrigger をセット)
        animator.SetTrigger("DownTrigger");
    }

    /// <summary>
    /// ノックバックが終了したときに呼び出す内部メソッド
    /// </summary>
    private void EndKnockback()
    {
        isKnocked = false;
        knockbackTimer = 0f;
        knockbackVelocity = Vector3.zero;

        // プレイヤー操作を再度有効化
        playerMove.enabled = true;

        // アニメーターの DownTrigger をリセットして状態を戻す
        animator.ResetTrigger("DownTrigger");
        // 必要に応じて別のトリガーやフラグを設定し、Idle/Run状態へ遷移させてください
    }
}
