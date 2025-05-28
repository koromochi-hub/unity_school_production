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
                PlayerStatus status = hit.GetComponent<PlayerStatus>();
                if (status != null)
                {
                    Vector3 knockbackDir = (hit.transform.position - transform.position).normalized;
                    status.TakeDamage(damage, knockbackDir, knockbackForce);
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
                    _ = trap.DelayedTrigger(0.1f);
                }
            }
        }
    }
}