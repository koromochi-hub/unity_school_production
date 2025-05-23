using UnityEngine;
public class FirePowder : TrapBase
{
    [SerializeField] private GameObject explosionEffectPrefab;
    private bool isTriggered = false;
    [Header("爆弾ステータス")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float knockbackForce = 10f;

    public override void Trigger()
    {
        if (isTriggered) return;
        isTriggered = true;

        Debug.Log("火薬が誘爆！");
        Explode();
    }

    private void Explode()
    {
        // 爆発エフェクト表示
        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        // 範囲内の情報を取得
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            // プレイヤーだった場合
            if (hit.CompareTag("Player"))
            {
                PlayerStatus status = hit.GetComponent<PlayerStatus>();
                if (status != null)
                {
                    Vector3 knockbackDir = (hit.transform.position - transform.position).normalized;
                    status.TakeDamage(damage, knockbackDir, knockbackForce);
                }
            }
            
            // 火薬だった場合
            FirePowder firePowder = hit.GetComponent<FirePowder>();
            
            if (firePowder != null)
            {
                firePowder.Trigger(); // 誘爆
            }
        }

        base.Trigger();
    }
}
