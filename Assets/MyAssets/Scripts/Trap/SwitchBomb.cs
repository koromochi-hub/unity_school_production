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
        Debug.Log("�X�C�b�`���e���N���I");

        // �����G�t�F�N�g�\��
        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        DealDamageToPlayers(hits);
        TriggerNearbyBombs(hits);

        base.Trigger(); // �g���b�v�폜�Ȃǋ��ʏ���
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}