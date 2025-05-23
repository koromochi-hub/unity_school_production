using UnityEngine;

public class LandMine : TrapBase
{
    [SerializeField] private GameObject explosionEffectPrefab;
    [Header("���e�X�e�[�^�X")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float knockbackForce = 10f;

    //public void SetOwner(PlayerStatus player)
    //{
    //    owner = player;
    //}

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

        // �͈͓��̃v���C���[�Ƀ_���[�W
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

    // �f�o�b�O�p�F�����͈͊m�F
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }
}
