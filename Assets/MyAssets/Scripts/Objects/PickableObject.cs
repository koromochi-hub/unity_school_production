using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [Header("���̃I�u�W�F�N�g���^����_���[�W��")]
    [SerializeField] private int damageAmount = 10;

    // ������ꂽ��Ԃ��ǂ����������t���O
    private bool isThrown = false;

    // �C�ӂŁA������ꂽ��Ɉ�x�ł��_���[�W������N��������
    // �ȍ~�͂����_���[�W��^���Ȃ��悤�ɂ���ꍇ�� �� ��ǉ�
    private bool hasDealtDamage = false;

    private void Awake()
    {
        // �������Ȃ�
    }

    /// <summary>
    /// GrabThrow �X�N���v�g����u������u�ԁv�ɌĂяo���āA
    /// ���̃I�u�W�F�N�g���h������ꂽ�h��ԂɈڍs���܂��B
    /// </summary>
    public void SetThrown(bool thrown)
    {
        isThrown = thrown;
        if (thrown)
        {
            hasDealtDamage = false; // ���������̂��тɃ_���[�W�t���O�����Z�b�g
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �� �������Ă��Ȃ��i�����n�ʂɗ����Ă���j�Ƃ��͉������Ȃ�
        if (!isThrown) return;

        // �� ���łɃ_���[�W��^�����Ȃ�A�ēx�͗^���Ȃ��i�K�v�ɉ����āj
        if (hasDealtDamage) return;

        // �� �v���C���[�ɓ��������Ƃ������_���[�W��^����
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("�v���C���[��Hit�I");
            PlayerStatus playerStatus = collision.collider.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damageAmount);
                hasDealtDamage = true;
            }

            // ����������e�iPickableObject�j�����������Ȃ�ȉ����A���R�����g
            // Destroy(this.gameObject);
        }

        // �� �n�ʂ�ǂȂǑ��̂��̂ɏՓ˂����Ƃ��́A�����œ�����Ԃ��������ĐÎ~��Ԃɖ߂�
        if (collision.collider.CompareTag("Ground") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            // �Ԃ������u�Ԃɓ����t���O���I�t �� ����ȍ~�͏Փ˂��Ă��_���[�W��^���Ȃ�
            isThrown = false;
        }
    }
}
