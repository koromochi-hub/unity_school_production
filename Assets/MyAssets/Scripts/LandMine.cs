using UnityEngine;

public class LandMine : TrapBase
{
    [SerializeField] private GameObject explosionEffectPrefab;
    [Header("爆弾ステータス")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float knockbackForce = 10f;

    //public void SetOwner(PlayerStatus player)
    //{
    //    owner = player;
    //}

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが踏んだか確認
        if (other.CompareTag("Player"))
        {
            PlayerStatus target = other.GetComponent<PlayerStatus>();

            // 設置者以外が踏んだ場合に爆発
            if (target != null && target != owner) 
            {
                Explode();
            }
        }
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
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }
}
