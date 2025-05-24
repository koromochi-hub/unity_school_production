using UnityEngine;

public class LandMine : TrapBase
{
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private FirePowder firePowder;
    [Header("���e�X�e�[�^�X")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float knockbackForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[�����񂾂��m�F
        if (other.CompareTag("Player"))
        {
            PlayerStatus target = other.GetComponent<PlayerStatus>();

            // �ݒu�҈ȊO�����񂾏ꍇ�ɔ���
            if (target != null && target != owner) 
            {
                Explode();
            }
        }
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
            if (hit.CompareTag("Bomb"))
            {
                IExplodable explodable = hit.GetComponent<IExplodable>();
                if (explodable != null && explodable != (IExplodable)this)
                {
                    explodable.Trigger();
                }
            }
        }

        base.Trigger();
    }
}
