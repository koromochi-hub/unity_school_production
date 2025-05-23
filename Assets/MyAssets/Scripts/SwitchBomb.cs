using UnityEngine;

public class SwitchBomb : TrapBase
{
    [SerializeField] private GameObject explosionEffectPrefab;

    [Header("���e�X�e�[�^�X")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int damage = 18;
    [SerializeField] private float knockbackForce = 10f;

    private void Start()
    {
        SwitchBombManager.Instance.Register(this);
    }

    public override void Trigger()
    {
        Debug.Log("�X�C�b�`���e���N���I");
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
            FirePowder firePowder = hit.GetComponentInParent<FirePowder>();
            Debug.Log("firePowder :" + hit);
            if (firePowder != null)
            {
                firePowder.Trigger(); 
            }
        }

        base.Trigger();
    }

    // �f�o�b�O�p�F�����͈͊m�F
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}