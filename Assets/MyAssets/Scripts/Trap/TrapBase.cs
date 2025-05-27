using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class TrapBase : MonoBehaviour
{
    protected Vector2Int gridPos;
    protected PlayerStatus owner;
    protected PlayerTrapController trapController;
    protected bool hasExploded = false;
    protected int trapIndex;

    public bool HasExploded() => hasExploded;


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
        trapController?.OnTrapDestroyed(trapIndex);

        Destroy(gameObject);
    }

    public async UniTask DelayedTrigger(float delaySeconds)
    {
        if (hasExploded) return;

        await UniTask.Delay(TimeSpan.FromSeconds(delaySeconds));
        Trigger();
    }
}
