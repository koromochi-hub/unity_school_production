using UnityEngine;
public class FirePowder : BombBase, IExplodable
{
    [SerializeField] private GameObject explosionEffectPrefab;

    public override void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer, PlayerTrapController controller, int trapIndex)
    {
        base.Initialize(gridPos, ownerPlayer, controller, trapIndex);
    }

    public override void Trigger()
    {
        if (hasExploded) return;
        Explode();
    }

    protected override void Explode()
    {
        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        base.Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}