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
        Debug.Log("�X�C�b�`���e���N���I");

        // �p�[�e�B�N����\��
        Vector3 explosionPosition = transform.position + Vector3.up * 1.2f;
        Instantiate(explosionEffectPrefab, explosionPosition, Quaternion.identity);

        // ����2�}�X�ɂ���G�v���C���[�Ƀ_���[�W
        DealDamageToNearbyEnemies();

        base.Trigger();
    }

    private void DealDamageToNearbyEnemies()
    {
        float radius = 2.5f; // 2�}�X�i�}�X��1x1�ŁA������0.5f����̂��߁j
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") && hit.gameObject != this.gameObject) // �����ȊO�̃v���C���[
            {
                PlayerStatus status = hit.GetComponent<PlayerStatus>();
                if (status != null)
                {
                    Vector3 knockbackDir = (hit.transform.position - transform.position).normalized;
                    status.TakeDamage(20, knockbackDir, 10f); // 20�_���[�W & �m�b�N�o�b�N10
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