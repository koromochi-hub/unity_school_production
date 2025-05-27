using UnityEngine;

public class PlayerTrapController : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private GameObject[] trapPrefabs;
    [SerializeField] private GridManager gridManager;

    private PlayerStatus owner;
    private int currentTrapIndex = 0;
    private int[] trapMaxCounts = new int[] { 1, 4, 4 };
    private int[] trapCurrentCounts;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.Character.SelectTrapL.performed += context => SwitchTrap(-1);
        playerInput.Character.SelectTrapR.performed += context => SwitchTrap(1);
        playerInput.Character.SetTrap.performed += context => SetTrap();
        playerInput.Character.ActivateBomb.performed += context => ActivateBomb();
    }

    private void Start()
    {
        owner = GetComponent<PlayerStatus>();
        trapCurrentCounts = new int[trapPrefabs.Length];
    }

    private void SwitchTrap(int direction)
    {
        currentTrapIndex = (currentTrapIndex + direction + trapPrefabs.Length) % trapPrefabs.Length;
        Debug.Log($"Trap switched to index: {currentTrapIndex}");
    }

    private void SetTrap()
    {
        Vector2Int gridPos = gridManager.WorldToGrid(transform.position);

        if (trapCurrentCounts[currentTrapIndex] >= trapMaxCounts[currentTrapIndex])
        {
            Debug.Log("設置上限に達しています！");
            return;
        }

        if (gridManager.CanSetTrap(gridPos))
        {
            gridManager.PlaceTrap(gridPos, trapPrefabs[currentTrapIndex], owner,this, currentTrapIndex);
            trapCurrentCounts[currentTrapIndex]++;
        }
        else
        {
            Debug.Log("そのマスには設置できません！");
        }
    }

    private void ActivateBomb()
    {
        SwitchBombManager.Instance.TriggerAll(owner);
    }

    public void OnTrapDestroyed(int trapIndex)
    {
        Debug.Log("PlayerTrapControllerへ移動しました");
        trapCurrentCounts[trapIndex] = Mathf.Max(0, trapCurrentCounts[trapIndex] - 1);
        
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();
}
