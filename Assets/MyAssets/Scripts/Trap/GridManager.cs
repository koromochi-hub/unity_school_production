using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // �g���b�v�폜���ɒʒm����C�x���g��錾
    public event Action<Vector2Int> OnTrapRemoved;

    // �ݒu�ς݃g���b�v���Ǘ����鎫��(Group) �� key=�O���b�h���W�Avalue=Trap�{��(GameObject)
    private Dictionary<Vector2Int, GameObject> placedTraps = new Dictionary<Vector2Int, GameObject>();

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.z);
        return new Vector2Int(x, y);
    }

    public bool CanSetTrap(Vector2Int gridPos)
    {
        return !placedTraps.ContainsKey(gridPos);
    }

    public void PlaceTrap(Vector2Int gridPos, GameObject trapPrefab, PlayerStatus owner, PlayerTrapController trapController, int trapTypeIndex)
    {
        if(CanSetTrap(gridPos))
        {
            float worldX = gridPos.x + 0.5f;
            float worldZ = gridPos.y + 0.5f;

            float worldY = owner.transform.position.y + 0.5f;

            Vector3 worldPos = new Vector3(worldX, worldY, worldZ);
            GameObject trap = Instantiate(trapPrefab, worldPos, Quaternion.identity);


            // �v���C���[���Ƃɔ��肵�ă��C���[��ݒ�
            int trapLayer = -1;

            if (owner.playerId == 0)
                trapLayer = LayerMask.NameToLayer("Trap_P1");
            else if (owner.playerId == 1)
                trapLayer = LayerMask.NameToLayer("Trap_P2");

            // ���C���[(0 �` 31)�����ݒ�̏ꍇ
            if (trapLayer != -1)
                SetLayerRecursively(trap, trapLayer);

            TrapBase trapbase = trap.GetComponent<TrapBase>();
            trapbase.Initialize(gridPos, owner, trapController, trapTypeIndex);

            placedTraps[gridPos] = trap;
        }
    }

    // �g���b�v�̎q�I�u�W�F�N�g�ɂ����C���[�ǉ�
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    /// <summary>
    /// �w����W�ɂ���g���b�v���폜����B
    /// ������ placedTraps ���珜�O���AGameObject �� Destroy ���A����ɃC�x���g�𔭉΂���B
    /// </summary>
    public void ClearTrap(Vector2Int gridPos)
    {
        if (!placedTraps.ContainsKey(gridPos)) return;

        // �@ Scene ��ɒu����Ă���g���b�v�{�̂� Destroy
        GameObject trap = placedTraps[gridPos];
        if (trap != null)
        {
            Destroy(trap);
        }

        // �A ����������L�[�����O
        placedTraps.Remove(gridPos);

        // �B �g���b�v�폜�C�x���g�𔭉΁i�w�ǂ��Ă���X�N���v�g�ɒʒm�j
        OnTrapRemoved?.Invoke(gridPos);
    }

    /// <summary>
    /// �g���b�v�����݂���ꍇ�A���̃g���b�v�i GameObject �j�� trap �Ƃ����ϐ��ɑ�����A true ��Ԃ��B
    /// </summary>
    public bool TryGetTrapData(Vector2Int gridPos, out GameObject trap)
    {
        return placedTraps.TryGetValue(gridPos, out trap);
    }
}