using UnityEngine;
public class FirePowder : BombBase, IExplodable
{
    [SerializeField] private GameObject explosionEffectPrefab;

    public override void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer, PlayerTrapController controller, int trapIndex)
    {
        base.Initialize(gridPos, ownerPlayer, controller, trapIndex);
        Debug.Log("スイッチ爆弾を初期化しました");
    }

    public override void Trigger()
    {
        if (hasExploded) return;
        hasExploded = true;
        Explode();
    }

    protected override void Explode()
    {
        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        DealDamageToPlayers(hits);
        TriggerNearbyBombs(hits);

        base.Trigger();
    }
}