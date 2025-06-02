using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableObject : MonoBehaviour
{
    [Header("投擲ダメージ設定")]
    [Tooltip("このオブジェクトでヒットしたときに与えるダメージ量 (整数にキャストされます)")]
    public float damageAmount = 10f;

    [Tooltip("当たった相手に与えるノックバックの力 (調整可)")]
    public float knockbackForce = 5f;

    private bool hasBounced = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    /// <summary>リスポーンなどで再利用する場合、バウンスフラグをリセットする</summary>
    public void ResetBounce()
    {
        hasBounced = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ① 床面 (タグが "Ground") に当たったら一度バウンスとみなし、以降はダメージ判定しない
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasBounced = true;
            return;
        }

        // ② まだバウンス前の状態で、プレイヤーに当たったらダメージを与える
        if (!hasBounced && collision.gameObject.CompareTag("Player"))
        {
            // (a) PlayerStatus を取得
            var playerStatus = collision.gameObject.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                // (b) ノックバック方向を計算:
                //     「当たったプレイヤーの位置 - 投げられたオブジェクトの現在位置」で方向を求める
                Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;

                // (c) ダメージ量は int にキャスト
                int dmg = Mathf.RoundToInt(damageAmount);

                // (d) PlayerStatus.TakeDamage(int, Vector3, float) を呼び出す
                playerStatus.TakeDamage(dmg, knockbackDirection);
            }

            // (e) ヒットしたらオブジェクトを破棄
            Destroy(gameObject);
        }
    }
}
