using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    protected Vector2Int gridPos;
    protected PlayerStatus owner;
    protected PlayerTrapController trapController;
    protected bool hasExploded = false;
    protected int trapIndex;

    public virtual void Initialize(Vector2Int gridPos, PlayerStatus ownerPlayer, PlayerTrapController controller, int trapTypeIndex)
    {
        this.gridPos = gridPos;
        owner = ownerPlayer;
        trapController = controller;
        trapIndex = trapTypeIndex;
    }

    public virtual void Trigger()
    {
        if (hasExploded) return;

        hasExploded = true;
        GridManager.Instance.ClearTrap(gridPos);

        // TrapController にカウント減らすよう通知
        trapController?.OnTrapDestroyed(trapIndex);

        Destroy(gameObject);
    }
}
