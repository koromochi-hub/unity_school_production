using UnityEngine;

public abstract class TrapBase : MonoBehaviour, IExplodable
{
    protected Vector2Int gridPos;
    protected PlayerStatus owner;


    public virtual void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer)
    {
        this.gridPos = gridPos;
        owner = ownerPlayer;
    }

    public virtual void Trigger()
    {
        // �O���b�h�����N���A
        GridManager.Instance.ClearTrap(gridPos);
        Destroy(gameObject);
    }
}
