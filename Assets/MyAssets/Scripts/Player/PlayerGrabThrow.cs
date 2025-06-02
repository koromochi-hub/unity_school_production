using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabThrow : MonoBehaviour
{
    [Header("�͂ށ^������ �ݒ�")]
    [SerializeField, Tooltip("�E����I�u�W�F�N�g�����m���锼�a")]
    private float maxPickupDistance = 1.5f;

    [SerializeField, Tooltip("�I�u�W�F�N�g���������Ƃ��A�^�b�`����z�[���h�|�C���g")]
    private Transform holdPoint;

    [SerializeField, Tooltip("�����������Ƃ��̍ő哊����")]
    private float maxThrowForce = 15f;

    [SerializeField, Tooltip("�����͂����߂鑬�� (1�b�� maxThrowForce �܂ŏグ��Ȃ� maxThrowForce / 1f)")]
    private float throwChargeSpeed = 15f;

    // --- �����t�B�[���h ---
    private GameObject heldObject = null;
    private Rigidbody heldRigidbody = null;
    private bool isCharging = false;
    private float currentThrowForce = 0f;
    private PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        if (holdPoint == null)
        {
            Debug.LogError("PlayerGrabThrow: HoldPoint �� Inspector �ɃZ�b�g����Ă��܂���B");
        }
    }

    /// <summary>
    /// PlayerInput �́uGrabThrow�v�A�N�V��������Ăяo����郁�\�b�h
    /// </summary>
    public void OnGrabThrow(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (heldObject == null)
            {
                TryPickUp();
            }
            else
            {
                // ���łɎ����Ă���Ȃ瓊���邽�߃`���[�W�J�n
                isCharging = true;
                currentThrowForce = 0f;
            }
        }
        else if (context.canceled)
        {
            if (heldObject != null && isCharging)
            {
                ThrowHeldObject();
                isCharging = false;
                currentThrowForce = 0f;
            }
        }
    }

    private void Update()
    {
        // �`���[�W���ŃI�u�W�F�N�g�������Ă���Ȃ�A�͂𗭂߂�
        if (isCharging && heldObject != null)
        {
            currentThrowForce += throwChargeSpeed * Time.deltaTime;
            currentThrowForce = Mathf.Min(currentThrowForce, maxThrowForce);
        }

        // �����Ă���I�u�W�F�N�g������� HoldPoint �ɒǏ]
        if (heldObject != null && holdPoint != null)
        {
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;
        }
    }

    /// <summary>
    /// �߂��� "Throwable" �^�O�t���I�u�W�F�N�g��T���Ē͂�
    /// </summary>
    private void TryPickUp()
    {
        Debug.Log("TryPickUp()���Ă΂�܂���");
        Collider[] hits = Physics.OverlapSphere(transform.position, maxPickupDistance);
        float closestDist = Mathf.Infinity;
        GameObject nearest = null;

        foreach (var col in hits)
        {
            if (col.gameObject.CompareTag("Throwable"))
            {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    nearest = col.gameObject;
                }
            }
        }

        if (nearest != null)
        {
            PickUp(nearest);
        }
    }

    /// <summary>
    /// ���ۂɃI�u�W�F�N�g��͂ޏ���
    /// </summary>
    private void PickUp(GameObject obj)
    {
        heldObject = obj;
        heldRigidbody = obj.GetComponent<Rigidbody>();

        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = true;
            heldRigidbody.linearVelocity = Vector3.zero;
            heldRigidbody.angularVelocity = Vector3.zero;
        }

        // PickableObject ���̃o�E���X�����Z�b�g
        var pickable = obj.GetComponent<PickableObject>();
        if (pickable != null)
        {
            pickable.ResetBounce();
        }

        // HoldPoint �ɃZ�b�g���Đe�q�֌W�����
        obj.transform.position = holdPoint.position;
        obj.transform.rotation = holdPoint.rotation;
        obj.transform.SetParent(holdPoint);
    }

    /// <summary>
    /// �I�u�W�F�N�g�𓊂��鏈��
    /// </summary>
    private void ThrowHeldObject()
    {
        if (heldObject == null) return;

        // �e�q�֌W������
        heldObject.transform.SetParent(null);

        // Rigidbody ��ʏ탂�[�h�ɖ߂�
        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = false;
        }

        // �y���������� transform.forward �ɂ���z
        Vector3 throwDir = transform.forward;

        if (heldRigidbody != null)
        {
            heldRigidbody.AddForce(throwDir * currentThrowForce, ForceMode.Impulse);
        }

        heldObject = null;
        heldRigidbody = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxPickupDistance);
    }
}
