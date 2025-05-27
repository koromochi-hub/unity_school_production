using UnityEngine;

public class SwitchBomb : BombBase, IExplodable
{
    [SerializeField] private GameObject explosionEffectPrefab;

    private void Start()
    {
        SwitchBombManager.Instance.Register(this, owner);
    }

    public override void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer, PlayerTrapController controller, int trapIndex)
    {
        base.Initialize(gridPos, ownerPlayer, controller, trapIndex);
    }

    public override void Trigger()
    {
        if (hasExploded) return;

        SwitchBombManager.Instance.Unregister(this, owner);
        Explode();
        Destroy(gameObject);
    }

    protected override void Explode()
    {
        Debug.Log("�X�C�b�`���e���N���I");
        
        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        // �����ƃ_���[�W������ BombBase ���őΉ�
        base.Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
