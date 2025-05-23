using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    protected Vector2Int gridPos;
    protected PlayerStatus owner;
    protected bool hasExploded = false;

    public virtual void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer)
    {
        this.gridPos = gridPos;
        owner = ownerPlayer;
    }

    public virtual void Trigger()
    {
        // グリッド情報をクリア
        GridManager.Instance.ClearTrap(gridPos);
        Destroy(gameObject);
    }
}

