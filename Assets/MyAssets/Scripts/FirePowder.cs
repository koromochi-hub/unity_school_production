using UnityEngine;
public class FirePowder : TrapBase
{
    public override void Trigger()
    {
        Debug.Log("‰Î–ò‚ª—U”šI");
        Explode();
    }

    private void Explode()
    {
        base.Trigger();
    }
}
