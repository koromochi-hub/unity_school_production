using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerTrapController : MonoBehaviour
{
    [SerializeField] private GameObject[] trapPrefabs;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject playerPrefab;

    private PlayerStatus owner;
    private int currentTrapIndex = 0;
    private int[] trapMaxCounts = new int[] { 1, 4, 4 };
    private int[] trapCurrentCounts;

    // �C�x���g�錾�F������ (�I�𒆂̃C���f�b�N�X, ���݌�, �ő��)
    public event Action<int, int, int> OnTrapSwitched;

    public int CurrentIndex => currentTrapIndex;
    public int GetCurrentCount() => trapCurrentCounts[currentTrapIndex];
    public int GetMaxCount() => trapMaxCounts[currentTrapIndex];

    private void Awake()
    {
        owner = GetComponent<PlayerStatus>();
        trapCurrentCounts = new int[trapPrefabs.Length];

        // �������������ő吔�Ɠ����ɃZ�b�g
        for (int i = 0; i < trapMaxCounts.Length; i++)
        {
            trapCurrentCounts[i] = trapMaxCounts[i];
        }

        if (gridManager == null)
            gridManager = FindFirstObjectByType<GridManager>();

        // ������Ԃ��ʒm
        NotifyTrapSwitched();
    }

    public void OnSelectTrapL(InputAction.CallbackContext context)
    {
        if (context.performed) SwitchTrap(-1);
    }

    public void OnSelectTrapR(InputAction.CallbackContext context)
    {
        if (context.performed) SwitchTrap(1);
    }

    public void OnSetTrap(InputAction.CallbackContext context)
    {
        if (context.performed) SetTrap();
    }

    public void OnActivateBomb(InputAction.CallbackContext context)
    {
        if (context.performed) ActivateBomb();
    }

    private void SwitchTrap(int direction)
    {
        currentTrapIndex = (currentTrapIndex + direction + trapPrefabs.Length) % trapPrefabs.Length;
        NotifyTrapSwitched();
    }

    private void NotifyTrapSwitched()
    {
        int cur = trapCurrentCounts[currentTrapIndex];
        int max = trapMaxCounts[currentTrapIndex];
        OnTrapSwitched?.Invoke(currentTrapIndex, cur, max);
    }

    private void SetTrap()
    {
        Vector2Int gridPos = gridManager.WorldToGrid(transform.position);

        // ���������O�Ȃ�ݒu�s��
        if (trapCurrentCounts[currentTrapIndex] <= 0)
            return;

        if (gridManager.CanSetTrap(gridPos))
        {
            gridManager.PlaceTrap(gridPos, trapPrefabs[currentTrapIndex], owner, this, currentTrapIndex);

            // �ݒu���Ƀf�N�������g
            trapCurrentCounts[currentTrapIndex]--;
            NotifyTrapSwitched();
        }
    }

    private void ActivateBomb()
    {
        SwitchBombManager.Instance.TriggerAll(owner);
    }

    public void OnTrapDestroyed(int trapIndex)
    {
        // �j�󎞂ɃC���N�������g�i����͍ő吔�܂Łj
        trapCurrentCounts[trapIndex] = Mathf.Min(trapCurrentCounts[trapIndex] + 1, trapMaxCounts[trapIndex]);
        if (trapIndex == currentTrapIndex)
            NotifyTrapSwitched();
    }
}
