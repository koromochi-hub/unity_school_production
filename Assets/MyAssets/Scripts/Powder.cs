using UnityEngine;
public class Powder : TrapBase
{
    public override void Trigger()
    {
        Debug.Log("�Ζ򂪗U���I");
        Explode();
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}
