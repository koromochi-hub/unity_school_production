using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(Animator))]
public class PlayerStatus : MonoBehaviour
{
    public int playerId;

    [Header("HP�ݒ�")]
    public int maxHP = 100;
    private int currentHP;

    [Header("�m�b�N�o�b�N�ݒ�")]
    [Tooltip("�������̃m�b�N�o�b�N���x")]
    [SerializeField] private float knockbackForce = 8f;

    [Tooltip("������̃m�b�N�o�b�N���x")]
    [SerializeField] private float knockbackUpForce = 5f;

    [Tooltip("�m�b�N�o�b�N���L���Ȏ��� (�b)")]
    [SerializeField] private float knockbackDuration = 1.0f;

    // �����L���b�V��
    private Rigidbody rb;
    private Animator animator;
    private PlayerMove playerMove;

    // �m�b�N�o�b�N�p
    private bool isKnocked = false;
    private float knockbackTimer = 0f;
    private Vector3 knockbackVelocity = Vector3.zero;

    private void Awake()
    {
        // �K�{�R���|�[�l���g���L���b�V��
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();

        if (rb == null)
            Debug.LogError("PlayerStatus requires a Rigidbody.");
        if (animator == null)
            Debug.LogError("PlayerStatus requires an Animator.");
        if (playerMove == null)
            Debug.LogError("PlayerStatus requires a PlayerMove.");

        currentHP = maxHP;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        // �m�b�N�o�b�N���ł���΁Avelocity ���Œ肵������
        if (isKnocked)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = knockbackVelocity;

            if (knockbackTimer <= 0f)
            {
                EndKnockback();
            }
        }
    }

    /// <summary>
    /// �_���[�W���󂯂��Ƃ��ɌĂяo��
    /// damage: �^����_���[�W�� (����)
    /// knockbackDirection: XZ���ʏ�̕����x�N�g�� (Y=0����)
    /// </summary>
    public void TakeDamage(int damage, Vector3 knockbackDirection)
    {
        // HP ���Z
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        Debug.Log($"{gameObject.name} �� {damage} �_���[�W���󂯂��B�c��HP: {currentHP}");

        // �m�b�N�o�b�N���J�n
        StartKnockback(knockbackDirection);
    }

    /// <summary>
    /// �m�b�N�o�b�N���J�n����������\�b�h
    /// </summary>
    private void StartKnockback(Vector3 direction)
    {
        if (isKnocked) return; // ���łɃm�b�N�o�b�N���Ȃ疳��

        isKnocked = true;
        knockbackTimer = knockbackDuration;

        // �v���C���[����𖳌��ɂ���
        playerMove.enabled = false;

        // �������̑��x
        Vector3 horizVel = direction.normalized * knockbackForce;
        // ������̑��x
        Vector3 upVel = Vector3.up * knockbackUpForce;
        // �����x�N�g��
        knockbackVelocity = horizVel + upVel;

        // Rigidbody �ɏ�����ݒ�
        rb.linearVelocity = knockbackVelocity;

        // �_�E���A�j���[�V�������Đ� (DownTrigger ���Z�b�g)
        animator.SetTrigger("DownTrigger");
    }

    /// <summary>
    /// �m�b�N�o�b�N���I�������Ƃ��ɌĂяo���������\�b�h
    /// </summary>
    private void EndKnockback()
    {
        isKnocked = false;
        knockbackTimer = 0f;
        knockbackVelocity = Vector3.zero;

        // �v���C���[������ēx�L����
        playerMove.enabled = true;

        // �A�j���[�^�[�� DownTrigger �����Z�b�g���ď�Ԃ�߂�
        animator.ResetTrigger("DownTrigger");
        // �K�v�ɉ����ĕʂ̃g���K�[��t���O��ݒ肵�AIdle/Run��Ԃ֑J�ڂ����Ă�������
    }
}
