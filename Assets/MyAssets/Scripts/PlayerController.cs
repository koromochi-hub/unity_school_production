using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform tf;
    [SerializeField] private Animator anim;
    [SerializeField] private GridManager gridManager;

    [Header("�ړ����x"), SerializeField]
    private float moveSpeed = 5f;

    [Header("㩃v���n�u�i3��ށj")]
    [SerializeField] private GameObject[] trapPrefabs;

    private int currentTrapIndex = 0;

    private void Update()
    {
        Movement();
        AnimRunning();

        // R1: �E�ɐ؂�ւ�
        if (Input.GetKeyDown(KeyCode.E)) // ��: R1�ɑΉ��i�ύX�j
        {
            currentTrapIndex = (currentTrapIndex + 1) % trapPrefabs.Length;
        }

        // L1: ���ɐ؂�ւ�
        if (Input.GetKeyDown(KeyCode.Q)) // ��: L1�ɑΉ��i�ύX�j
        {
            currentTrapIndex = (currentTrapIndex - 1 + trapPrefabs.Length) % trapPrefabs.Length;
        }

        // ���{�^���i��: Space�j�Őݒu
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetTrap();
        }

        if (Input.GetKeyDown(KeyCode.B)) // PS�R���g���[���[�́~�ɑ���
        {
            SwitchBombManager.Instance.TriggerNext();
        }
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D or ��/��
        float v = Input.GetAxisRaw("Vertical");   // W/S or ��/��

        // ���͕����x�N�g���iXZ���ʁj
        Vector3 inputDirection = new Vector3(h, 0f, v).normalized;

        float currentY = rb.linearVelocity.y;

        // ���͕����Ɉړ�
        Vector3 moveVelocity = inputDirection* moveSpeed;
        moveVelocity.y = currentY;

        rb.linearVelocity = moveVelocity;

        // �ړ����ɓ��͕����Ɍ�����ς���
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
        // �v���C���[�̈ʒu����O���b�h���W���擾
        Vector2Int gridPos = gridManager.WorldToGrid(transform.position);

        if (gridManager.CanSetTrap(gridPos))
        {
            gridManager.PlaceTrap(gridPos, trapPrefabs[currentTrapIndex]);
        }
        else
        {
            Debug.Log("���łɂ��̃}�X��㩂�����܂��I");
        }
    }
}
