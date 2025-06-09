using UnityEngine.InputSystem;
using UnityEngine;
using System;

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

    private void Awake()
    {
        owner = GetComponent<PlayerStatus>();
        trapCurrentCounts = new int[trapPrefabs.Length];
        if (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
        }

        // 初期状態も通知しておく
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

    // 通知用メソッド
    private void NotifyTrapSwitched()
    {
        int cur = trapCurrentCounts[currentTrapIndex];
        int max = trapMaxCounts[currentTrapIndex];
        OnTrapSwitched?.Invoke(currentTrapIndex, cur, max);
    }

    // 罠を設置／破壊して個数が変わるたびにも通知
    private void SetTrap()
    {
        Vector2Int gridPos = gridManager.WorldToGrid(transform.position);

        if (trapCurrentCounts[currentTrapIndex] >= trapMaxCounts[currentTrapIndex]) return;

        if (gridManager.CanSetTrap(gridPos))
        {
            gridManager.PlaceTrap(gridPos, trapPrefabs[currentTrapIndex], owner, this, currentTrapIndex);
            trapCurrentCounts[currentTrapIndex]++;
        }

        trapCurrentCounts[currentTrapIndex]++;
        NotifyTrapSwitched();
    }

    private void ActivateBomb()
    {
        SwitchBombManager.Instance.TriggerAll(owner);
    }

    public void OnTrapDestroyed(int trapIndex)
    {
        trapCurrentCounts[trapIndex] = Mathf.Max(0, trapCurrentCounts[trapIndex] - 1);
        if (trapIndex == currentTrapIndex)
            NotifyTrapSwitched();
    }
}
