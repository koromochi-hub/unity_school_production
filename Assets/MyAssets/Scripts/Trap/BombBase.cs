using UnityEngine;

public abstract class BombBase : TrapBase
{
    [SerializeField] protected float igniteRadius = 2.5f;

    protected bool hasExploded = false;

    public override void Trigger()
    {
        if (hasExploded) return;
        hasExploded = true;

        // �����������Ă�
        Explode();

        // �O���b�h����폜 & ���g���폜
        base.Trigger();
    }

    protected abstract void Explode();

    protected void BombIgnition()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, igniteRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Bomb"))
            {
                IExplodable explodable = hit.GetComponent<IExplodable>();
                if (explodable != null && (Object)explodable != this)
                {
                    explodable.Trigger();
                }
            }
        }
    }
}