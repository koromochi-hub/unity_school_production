using UnityEngine;

public class LandMine : BombBase, IExplodable
{
    [SerializeField] private GameObject explosionEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが踏んだか確認
        if (other.CompareTag("Player"))
        {
            PlayerStatus target = other.GetComponent<PlayerStatus>();

            // 設置者以外が踏んだ場合に爆発
            if (target != null && target != owner) 
            {
                Trigger();
            }
        }
    }

    public override void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer, PlayerTrapController controller, int trapIndex)
    {
        base.Initialize(gridPos, ownerPlayer, controller, trapIndex);
        Debug.Log("地雷を初期化しました");
    }

    public override void Trigger()
    {
        if (hasExploded) return;
        hasExploded = true;
        Explode();
    }

    protected override void Explode()
    {
        Debug.Log("地雷が爆発！");

        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        DealDamageToPlayers(hits);
        TriggerNearbyBombs(hits);

        base.Trigger();
    }
}
