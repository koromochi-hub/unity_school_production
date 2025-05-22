using UnityEngine;

public class LandMine : TrapBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // �܂��̓v���C���[�ȊO
        {
            Trigger();
        }
    }

    public override void Trigger()
    {
        Debug.Log("�n���������I");
        Explode();
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}
