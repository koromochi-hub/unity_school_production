using UnityEngine;

public class LandMine : BombBase, IExplodable
{
    [SerializeField] private GameObject explosionEffectPrefab;

    public override void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer, PlayerTrapController controller, int trapIndex)
    {
        base.Initialize(gridPos, ownerPlayer, controller, trapIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[�����񂾂��m�F
        if (other.CompareTag("Player"))
        {
            PlayerStatus target = other.GetComponent<PlayerStatus>();

            // �ݒu�҈ȊO�����񂾏ꍇ�ɔ���
            if (target != null && target != owner) 
            {
                if (hasExploded) return;

                Trigger();
            }
        }
    }

    public override void Trigger()
    {
        if (hasExploded) return;
        Explode();
    }

    protected override void Explode()
    {
        Debug.Log("�n���������I");

        Instantiate(explosionEffectPrefab, transform.position + Vector3.up * 1.2f, Quaternion.identity);

        base.Explode();
    }
}
