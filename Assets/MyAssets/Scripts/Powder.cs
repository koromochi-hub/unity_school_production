using UnityEngine;
public class Powder : TrapBase
{
    public override void Trigger()
    {
        Debug.Log("‰Î–ò‚ª—U”šI");
        Explode();
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}
