using UnityEngine;

public class SwitchBomb : TrapBase
{
    [SerializeField] private GameObject explosionEffectPrefab;

    [Header("爆弾ステータス")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int damage = 18;
    [SerializeField] private float knockbackForce = 10f;

    private void Start()
    {
        SwitchBombManager.Instance.Register(this);
    }

    public override void Trigger()
    {
        Debug.Log("スイッチ爆弾が起動！");
        Explode();
    }

    private void Explode()
    {
        // 爆発エフェクト表示
        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        // 範囲内のプレイヤーにダメージ
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
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

        base.Trigger();
    }

    // デバッグ用：爆発範囲確認
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}