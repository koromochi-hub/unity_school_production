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

    private void Awake()
    {
        owner = GetComponent<PlayerStatus>();
        trapCurrentCounts = new int[trapPrefabs.Length];
        if (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
        }
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
    }

    private void SetTrap()
    {
        Vector2Int gridPos = gridManager.WorldToGrid(transform.position);

        if (trapCurrentCounts[currentTrapIndex] >= trapMaxCounts[currentTrapIndex]) return;

        if (gridManager.CanSetTrap(gridPos))
        {
            gridManager.PlaceTrap(gridPos, trapPrefabs[currentTrapIndex], owner, this, currentTrapIndex);
            trapCurrentCounts[currentTrapIndex]++;
        }
    }

    private void ActivateBomb()
    {
        SwitchBombManager.Instance.TriggerAll(owner);
    }

    public void OnTrapDestroyed(int trapIndex)
    {
        trapCurrentCounts[trapIndex] = Mathf.Max(0, trapCurrentCounts[trapIndex] - 1);
    }

    //public void Initialize(GridManager gm)
    //{
    //    gridManager = gm;
    //}
}
