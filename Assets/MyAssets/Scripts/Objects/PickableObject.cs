using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableObject : MonoBehaviour
{
    [Header("�����_���[�W�ݒ�")]
    [Tooltip("���̃I�u�W�F�N�g�Ńq�b�g�����Ƃ��ɗ^����_���[�W�� (�����ɃL���X�g����܂�)")]
    public float damageAmount = 10f;

    [Tooltip("������������ɗ^����m�b�N�o�b�N�̗� (������)")]
    public float knockbackForce = 5f;

    private bool hasBounced = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    /// <summary>���X�|�[���Ȃǂōė��p����ꍇ�A�o�E���X�t���O�����Z�b�g����</summary>
    public void ResetBounce()
    {
        hasBounced = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �@ ���� (�^�O�� "Ground") �ɓ����������x�o�E���X�Ƃ݂Ȃ��A�ȍ~�̓_���[�W���肵�Ȃ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasBounced = true;
            return;
        }

        // �A �܂��o�E���X�O�̏�ԂŁA�v���C���[�ɓ���������_���[�W��^����
        if (!hasBounced && collision.gameObject.CompareTag("Player"))
        {
            // (a) PlayerStatus ���擾
            var playerStatus = collision.gameObject.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                // (b) �m�b�N�o�b�N�������v�Z:
                //     �u���������v���C���[�̈ʒu - ������ꂽ�I�u�W�F�N�g�̌��݈ʒu�v�ŕ��������߂�
                Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;

                // (c) �_���[�W�ʂ� int �ɃL���X�g
                int dmg = Mathf.RoundToInt(damageAmount);

                // (d) PlayerStatus.TakeDamage(int, Vector3, float) ���Ăяo��
                playerStatus.TakeDamage(dmg, knockbackDirection);
            }

            // (e) �q�b�g������I�u�W�F�N�g��j��
            Destroy(gameObject);
        }
    }
}
