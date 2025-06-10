using UnityEngine;

public abstract class BombBase : TrapBase, IExplodable
{
    [Header("���ʃX�e�[�^�X")]
    [SerializeField] protected float radius = 2.5f;
    [SerializeField] protected int damage = 25;
    [SerializeField] protected float knockbackForce = 10f; // �����̋��x
    [SerializeField] protected float upwardModifier = 2f;  // ������ւ̕␳

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
                // �@ �v���C���[�� Rigidbody ���擾���Ĕ�����^����
                Rigidbody playerRb = hit.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    // AddExplosionForce(�������x, ���S�n, �������a, ������␳)
                    playerRb.AddExplosionForce(knockbackForce, transform.position, radius, upwardModifier, ForceMode.Impulse);
                }

                // �A �_���[�W�����i�m�b�N�o�b�N�͕����G���W���C���ɂ���̂ŁATakeDamage �� damage �̂݁j
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
                // �������g�͏��O
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
