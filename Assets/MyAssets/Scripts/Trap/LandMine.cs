using UnityEngine;

public class LandMine : BombBase, IExplodable
{
    [SerializeField] private GameObject explosionEffectPrefab;

    public override void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer, PlayerTrapController controller, int trapIndex)
    {
        base.Initialize(gridPos, ownerPlayer, controller, trapIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが踏んだか確認
        if (other.CompareTag("Player"))
        {
            PlayerStatus target = other.GetComponent<PlayerStatus>();

            // 設置者以外が踏んだ場合に爆発
            if (target != null && target != owner) 
            {
                if (hasExploded) return;

                Trigger();
            }
        }
    }

    public override void Trigger()
    {
        if (hasExploded) return;
        Explode();
    }

    protected override void Explode()
    {
        Debug.Log("地雷が爆発！");

        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        base.Explode();
    }
}
