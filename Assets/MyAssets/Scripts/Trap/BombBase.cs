using UnityEngine;

public abstract class BombBase : TrapBase, IExplodable
{
    [Header("共通ステータス")]
    [SerializeField] protected float radius = 2.5f;
    [SerializeField] protected int damage = 25;
    [SerializeField] protected float knockbackForce = 10f; // 爆風の強度
    [SerializeField] protected float upwardModifier = 2f;  // 上方向への補正

    protected virtual void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        base.Trigger();
        DealDamageToPlayers(hits);
        TriggerNearbyBombs(hits);
    }

    protected void DealDamageToPlayers(Collider[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // ① プレイヤーの Rigidbody を取得して爆風を与える
                Rigidbody playerRb = hit.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    // AddExplosionForce(爆風強度, 爆心地, 爆風半径, 上方向補正)
                    playerRb.AddExplosionForce(knockbackForce, transform.position, radius, upwardModifier, ForceMode.Impulse);
                }

                // ② ダメージ処理（ノックバックは物理エンジン任せにするので、TakeDamage は damage のみ）
                PlayerStatus status = hit.GetComponent<PlayerStatus>();
                if (status != null)
                {
                    status.TakeDamage(damage);
                }
            }
        }
    }

    protected void TriggerNearbyBombs(Collider[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Bomb"))
            {
                // 自分自身は除外
                if (hit.gameObject == this.gameObject) continue;

                TrapBase trap = hit.GetComponent<TrapBase>();
                if (trap != null && !trap.HasExploded())
                {
                    _ = trap.DelayedTrigger(0.3f);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
