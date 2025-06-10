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

    // イベント宣言：引数は (選択中のインデックス, 現在個数, 最大個数)
    public event Action<int, int, int> OnTrapSwitched;

    public int CurrentIndex => currentTrapIndex;
    public int GetCurrentCount() => trapCurrentCounts[currentTrapIndex];
    public int GetMaxCount() => trapMaxCounts[currentTrapIndex];

    private void Awake()
    {
        owner = GetComponent<PlayerStatus>();
        trapCurrentCounts = new int[trapPrefabs.Length];

        // 初期所持数を最大数と同じにセット
        for (int i = 0; i < trapMaxCounts.Length; i++)
        {
            trapCurrentCounts[i] = trapMaxCounts[i];
        }

        if (gridManager == null)
            gridManager = FindFirstObjectByType<GridManager>();

        // 初期状態も通知
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

        // 所持数が０なら設置不可
        if (trapCurrentCounts[currentTrapIndex] <= 0)
            return;

        if (gridManager.CanSetTrap(gridPos))
        {
            gridManager.PlaceTrap(gridPos, trapPrefabs[currentTrapIndex], owner, this, currentTrapIndex);

            // 設置時にデクリメント
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
        // 破壊時にインクリメント（上限は最大数まで）
        trapCurrentCounts[trapIndex] = Mathf.Min(trapCurrentCounts[trapIndex] + 1, trapMaxCounts[trapIndex]);
        if (trapIndex == currentTrapIndex)
            NotifyTrapSwitched();
    }
}
