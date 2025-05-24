using UnityEngine;

public abstract class BombBase : TrapBase, IExplodable
{
    [Header("共通ステータス")]
    [SerializeField] protected float radius = 2.5f;
    [SerializeField] protected int damage = 25;
    [SerializeField] protected float knockbackForce = 10f;

    protected virtual void Explode()
{
    Collider[] hits = Physics.OverlapSphere(transform.position, radius);
    DealDamageToPlayers(hits);
    TriggerNearbyBombs(hits);

    base.Trigger(); // TrapBase 由来の共通後処理
}

    /// <summary>
    /// 範囲内のプレイヤーにダメージとノックバックを与える
    /// </summary>
    protected void DealDamageToPlayers(Collider[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerStatus status = hit.GetComponent<PlayerStatus>();
                if (status != null)
                {
                    Vector3 knockbackDir = (hit.transform.position - transform.position).normalized;
                    status.TakeDamage(damage, knockbackDir, knockbackForce);
                }
            }
        }
    }

    /// <summary>
    /// 範囲内の他の爆弾（Bombタグ）を誘爆させる
    /// </summary>
    protected void TriggerNearbyBombs(Collider[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Bomb"))
            {
                IExplodable explodable = hit.GetComponent<IExplodable>();
                if (explodable != null && (UnityEngine.Object)explodable != this)
                {
                    explodable.Trigger();
                }
            }
        }
    }
}