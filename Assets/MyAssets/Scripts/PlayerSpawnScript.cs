using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnScript : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;
    public PlayerStatus status;
    public Transform SpawnPoint1, SpawnPoint2;
    public GameObject Player1, Player2;

    private void Awake()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;

        Instantiate(Player1, SpawnPoint1.position, SpawnPoint1.rotation);
        Instantiate(Player2, SpawnPoint2.position, SpawnPoint2.rotation);
    }

    private void OnDestroy()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        status.playerId = playerInput.playerIndex;
    }
}
