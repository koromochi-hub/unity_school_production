using UnityEngine;
public class FirePowder : TrapBase
{
    [SerializeField] private GameObject explosionEffectPrefab;
    private bool isTriggered = false;
    [Header("���e�X�e�[�^�X")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float knockbackForce = 10f;

    public override void Trigger()
    {
        if (isTriggered) return;
        isTriggered = true;

        Debug.Log("�Ζ򂪗U���I");
        Explode();
    }

    private void Explode()
    {
        // �����G�t�F�N�g�\��
        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        // �͈͓��̏����擾
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            // �v���C���[�������ꍇ
            if (hit.CompareTag("Player"))
            {
                PlayerStatus status = hit.GetComponent<PlayerStatus>();
                if (status != null)
                {
                    Vector3 knockbackDir = (hit.transform.position - transform.position).normalized;
                    status.TakeDamage(damage, knockbackDir, knockbackForce);
                }
            }
            
            // �Ζ򂾂����ꍇ
            FirePowder firePowder = hit.GetComponent<FirePowder>();
            
            if (firePowder != null)
            {
                firePowder.Trigger(); // �U��
            }
        }

        base.Trigger();
    }
}
