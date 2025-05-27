using UnityEngine;

public class LandMine : BombBase, IExplodable
{
    [SerializeField] private GameObject explosionEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[�����񂾂��m�F
        if (other.CompareTag("Player"))
        {
            PlayerStatus target = other.GetComponent<PlayerStatus>();

            // �ݒu�҈ȊO�����񂾏ꍇ�ɔ���
            if (target != null && target != owner) 
            {
                Trigger();
            }
        }
    }

    public override void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer, PlayerTrapController controller, int trapIndex)
    {
        base.Initialize(gridPos, ownerPlayer, controller, trapIndex);
        Debug.Log("�n�������������܂���");
    }

    public override void Trigger()
    {
        if (hasExploded) return;
        hasExploded = true;
        Explode();
    }

    protected override void Explode()
    {
        Debug.Log("�n���������I");

        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        DealDamageToPlayers(hits);
        TriggerNearbyBombs(hits);

        base.Trigger();
    }
}
