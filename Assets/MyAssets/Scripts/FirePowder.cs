using UnityEngine;
public class FirePowder : TrapBase
{
    public override void Trigger()
    {
        Debug.Log("�Ζ򂪗U���I");
        Explode();
    }

    private void Explode()
    {
        base.Trigger();
    }
}
