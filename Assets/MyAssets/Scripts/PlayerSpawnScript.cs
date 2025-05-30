using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnScript : MonoBehaviour
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Transform SpawnPoint1, SpawnPoint2;

    private void Awake()
    {
        PlayerInput.Instantiate(playerPrefab1, controlScheme: null, pairWithDevice: null,
            splitScreenIndex: 0, position: SpawnPoint1.position, rotation: SpawnPoint1.rotation);

        PlayerInput.Instantiate(playerPrefab2, controlScheme: null, pairWithDevice: null,
            splitScreenIndex: 1, position: SpawnPoint2.position, rotation: SpawnPoint2.rotation);
    }
}
