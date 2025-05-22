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
        // ���ʂ̔�������������΂�����
        GridManager.Instance.ClearTrap(gridPos); // �� �����ŃO���b�h�����N���A
        Destroy(gameObject);
    }
}
