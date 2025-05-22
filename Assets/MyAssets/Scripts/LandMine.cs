using UnityEngine;

public class LandMine : TrapBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // またはプレイヤー以外
        {
            Trigger();
        }
    }

    public override void Trigger()
    {
        Debug.Log("地雷が爆発！");
        Explode();
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}
