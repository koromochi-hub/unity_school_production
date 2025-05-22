using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    protected Vector2Int gridPos;

    public virtual void Initialize(Vector2Int pos)
    {
        gridPos = pos;
    }


    public virtual void Trigger()
    {
        // 共通の爆発処理があればここに
        GridManager.Instance.ClearTrap(gridPos); // ← ここでグリッド情報をクリア
        Destroy(gameObject);
    }
}
