using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform tf;
    [SerializeField] private Animator anim;
    [SerializeField] private GridManager gridManager;

    [Header("移動速度"), SerializeField]
    private float moveSpeed = 5f;

    [Header("罠プレハブ（3種類）")]
    [SerializeField] private GameObject[] trapPrefabs;

    private int currentTrapIndex = 0;

    private void Update()
    {
        Movement();
        AnimRunning();

        // R1: 右に切り替え
        if (Input.GetKeyDown(KeyCode.E)) // 例: R1に対応（変更可）
        {
            currentTrapIndex = (currentTrapIndex + 1) % trapPrefabs.Length;
        }

        // L1: 左に切り替え
        if (Input.GetKeyDown(KeyCode.Q)) // 例: L1に対応（変更可）
        {
            currentTrapIndex = (currentTrapIndex - 1 + trapPrefabs.Length) % trapPrefabs.Length;
        }

        // □ボタン（例: Space）で設置
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetTrap();
        }

        if (Input.GetKeyDown(KeyCode.B)) // PSコントローラーの×に相当
        {
            SwitchBombManager.Instance.TriggerNext();
        }
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D or ←/→
        float v = Input.GetAxisRaw("Vertical");   // W/S or ↑/↓

        // 入力方向ベクトル（XZ平面）
        Vector3 inputDirection = new Vector3(h, 0f, v).normalized;

        float currentY = rb.linearVelocity.y;

        // 入力方向に移動
        Vector3 moveVelocity = inputDirection* moveSpeed;
        moveVelocity.y = currentY;

        rb.linearVelocity = moveVelocity;

        // 移動時に入力方向に向きを変える
        if (inputDirection != Vector3.zero)
            tf.rotation = Quaternion.LookRotation(inputDirection);
    }

    private void AnimRunning()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);
    }

    private void SetTrap()
    {
        // プレイヤーの位置からグリッド座標を取得
        Vector2Int gridPos = gridManager.WorldToGrid(transform.position);

        if (gridManager.CanSetTrap(gridPos))
        {
            gridManager.PlaceTrap(gridPos, trapPrefabs[currentTrapIndex]);
        }
        else
        {
            Debug.Log("すでにそのマスに罠があります！");
        }
    }
}
