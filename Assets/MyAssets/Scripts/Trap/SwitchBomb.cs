using UnityEngine;

public class SwitchBomb : BombBase, IExplodable
{
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject highlightPrefab;

    private void Start()
    {
        SwitchBombManager.Instance.Register(this, owner);
    }

    public override void Trigger()
    {
        if (hasExploded) return;
        hasExploded = true;

        SwitchBombManager.Instance.Unregister(this, owner);
        
        Explode();
    }

    protected override void Explode()
    {
        Debug.Log("スイッチ爆弾が起動！");

        // 爆発エフェクト表示
        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        DealDamageToPlayers(hits);
        TriggerNearbyBombs(hits);

        base.Trigger(); // トラップ削除など共通処理
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}