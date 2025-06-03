using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [Header("このオブジェクトが与えるダメージ量")]
    [SerializeField] private int damageAmount = 10;

    // 投げられた状態かどうかを示すフラグ
    private bool isThrown = false;

    // 任意で、投げられた後に一度でもダメージ判定を起こしたら
    // 以降はもうダメージを与えないようにする場合は ↓ を追加
    private bool hasDealtDamage = false;

    private void Awake()
    {
        // 何もしない
    }

    /// <summary>
    /// GrabThrow スクリプトから「投げる瞬間」に呼び出して、
    /// このオブジェクトが”投げられた”状態に移行します。
    /// </summary>
    public void SetThrown(bool thrown)
    {
        isThrown = thrown;
        if (thrown)
        {
            hasDealtDamage = false; // 投げ直しのたびにダメージフラグをリセット
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ● 投げられていない（ただ地面に落ちている）ときは何もしない
        if (!isThrown) return;

        // ● すでにダメージを与えたなら、再度は与えない（必要に応じて）
        if (hasDealtDamage) return;

        // ● プレイヤーに当たったときだけダメージを与える
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("プレイヤーにHit！");
            PlayerStatus playerStatus = collision.collider.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damageAmount);
                hasDealtDamage = true;
            }

            // 当たったら弾（PickableObject）を消したいなら以下をアンコメント
            // Destroy(this.gameObject);
        }

        // ● 地面や壁など他のものに衝突したときは、そこで投げ状態を解除して静止状態に戻す
        if (collision.collider.CompareTag("Ground") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            // ぶつかった瞬間に投げフラグをオフ → これ以降は衝突してもダメージを与えない
            isThrown = false;
        }
    }
}
