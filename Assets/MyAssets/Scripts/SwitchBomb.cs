using UnityEngine;

public class SwitchBomb : TrapBase
{
    [SerializeField] private GameObject explosionEffectPrefab;

    private void Start()
    {
        SwitchBombManager.Instance.Register(this);
    }

    public override void Trigger()
    {
        Debug.Log("スイッチ爆弾が起動！");

        // パーティクルを表示
        Vector3 explosionPosition = transform.position + Vector3.up * 1.2f;
        Instantiate(explosionEffectPrefab, explosionPosition, Quaternion.identity);

        // 周囲2マスにいる敵プレイヤーにダメージ
        DealDamageToNearbyEnemies();

        base.Trigger();
    }

    private void DealDamageToNearbyEnemies()
    {
        float radius = 2.5f; // 2マス（マスが1x1で、中央の0.5fが基準のため）
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") && hit.gameObject != this.gameObject) // 自分以外のプレイヤー
            {
                PlayerStatus status = hit.GetComponent<PlayerStatus>();
                if (status != null)
                {
                    Vector3 knockbackDir = (hit.transform.position - transform.position).normalized;
                    status.TakeDamage(20, knockbackDir, 10f); // 20ダメージ & ノックバック10
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}