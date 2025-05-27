using UnityEngine;

public class SwitchBomb : BombBase, IExplodable
{
    [SerializeField] private GameObject explosionEffectPrefab;

    private void Start()
    {
        SwitchBombManager.Instance.Register(this, owner);
    }

    public override void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer, PlayerTrapController controller, int trapIndex)
    {
        base.Initialize(gridPos, ownerPlayer, controller, trapIndex);
        Debug.Log("スイッチ爆弾を初期化しました");
    }

    public override void Trigger()
    {
        if (hasExploded) return;

        // 登録解除は重複起動を防ぐために最初に
        SwitchBombManager.Instance.Unregister(this, owner);

        Explode();
    }

    protected override void Explode()
    {
        Debug.Log("スイッチ爆弾が起動！");

        // 爆発エフェクト表示
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);
        }

        // 爆風とダメージ処理は BombBase 側で対応
        base.Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
