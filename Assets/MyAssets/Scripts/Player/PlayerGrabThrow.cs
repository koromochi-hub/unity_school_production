using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerGrabThrow : MonoBehaviour
{
    [Header("�͂ށiGrab�j�ݒ�")]
    [Tooltip("�͂߂�I�u�W�F�N�g�̃^�O (Pickable �Ȃ�)")]
    [SerializeField] private string pickableTag = "Pickable";

    [Tooltip("�͂ތ��o�͈� (���[�g��)")]
    [SerializeField] private float grabRange = 1.0f;

    [Tooltip("�͂ނƂ��Ɏg�����C���[�}�X�N (Pickable �I�u�W�F�N�g�̃��C���[)")]
    [SerializeField] private LayerMask pickableLayerMask;

    [Tooltip("�I�u�W�F�N�g�����ʒu")]
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform carryPoint;

    [Header("������iThrow�j�ݒ�")]
    [Tooltip("������Ƃ��̍ŏ�����")]
    [SerializeField] private float minThrowForce = 5f;

    [Tooltip("������Ƃ��̍ő叉��")]
    [SerializeField] private float maxThrowForce = 15f;

    [Tooltip("�{�^���������ɂ��ő�`���[�W���� (�b)")]
    [SerializeField] private float maxChargeTime = 1.5f;

    // ������������ �����L���b�V�� ������������
    private PlayerInput playerInput;
    private InputAction grabAction;

    private GameObject carryingObject = null;   // ���ݒ͂�ł���I�u�W�F�N�g
    private Rigidbody carryRb = null;            // ���̃I�u�W�F�N�g�� Rigidbody

    private float grabChargeTimer = 0f;       // �u�{�^���������v�ɂ�铊�����x�`���[�W�p�^�C�}�[
    private bool isChargingThrow = false;     // ���ݓ����鋭�x���`���[�W���Ă��邩

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            Debug.LogError("GrabThrow: PlayerInput ���A�^�b�`����Ă��܂���B");

        grabAction = playerInput.actions["GrabThrow"];
        if (grabAction == null)
            Debug.LogError("GrabThrow: PlayerInput �� ActionMap �� 'GrabThrow' ������܂���B");

        if (carryPoint == null)
            Debug.LogError("GrabThrow: carryPoint ���A�T�C������Ă��܂���B");
    }

    private void OnEnable()
    {
        grabAction.started += OnGrabStarted;
        grabAction.canceled += OnGrabCanceled;
    }

    private void OnDisable()
    {
        grabAction.started -= OnGrabStarted;
        grabAction.canceled -= OnGrabCanceled;
    }

    private void Update()
    {
        // (1) �����`���[�W�����F�{�^�������������Ă���Ԃ����^�C�}�[�𑝂₷
        if (isChargingThrow && carryingObject != null)
        {
            grabChargeTimer += Time.deltaTime;
            if (grabChargeTimer > maxChargeTime)
                grabChargeTimer = maxChargeTime;
        }
    }

    /// <summary>
    /// �O���u�p�{�^�����������u�� (started)
    /// �� �܂������͂�ł��Ȃ���� �g�͂݁h ���g���C
    /// �� ���łɒ͂�ł�����g�����h�̃`���[�W���J�n
    /// </summary>
    private void OnGrabStarted(InputAction.CallbackContext context)
    {
        if (carryingObject == null)
        {
            TryGrab();
        }
        else
        {
            // ���łɃI�u�W�F�N�g�������Ă��� �� �����̃`���[�W�J�n
            isChargingThrow = true;
            grabChargeTimer = 0f;
        }
    }

    /// <summary>
    /// �O���u�p�{�^���𗣂����u�� (canceled)
    /// �� �����I�u�W�F�N�g�������Ă���΁u������v���������s
    /// </summary>
    private void OnGrabCanceled(InputAction.CallbackContext context)
    {
        if (carryingObject != null && isChargingThrow)
        {
            ThrowObject();
        }

        isChargingThrow = false;
    }

    /// <summary>
    /// �t�߂� Pickable �I�u�W�F�N�g��T���Ē͂�
    /// </summary>
    private void TryGrab()
    {
        // 1) �茳�ʒu���� grabRange ���a�� OverlapSphere �����s
        Collider[] hits = Physics.OverlapSphere(grabPoint.position, grabRange, pickableLayerMask);
        if (hits.Length == 0) return;

        // 2) �ŏ��Ɍ������� Pickable �^�O�t���I�u�W�F�N�g��͂�
        GameObject target = null;
        foreach (var col in hits)
        {
            if (col.CompareTag(pickableTag))
            {
                target = col.gameObject;
                break;
            }
        }
        if (target == null) return;

        // 3) �͂ޏ����FRigidbody �� isKinematic �ɂ��Ď����グ��
        carryingObject = target;
        carryRb = carryingObject.GetComponent<Rigidbody>();
        if (carryRb != null)
        {
            carryRb.linearVelocity = Vector3.zero;
            carryRb.angularVelocity = Vector3.zero;
            carryRb.useGravity = false;
            carryRb.isKinematic = true;
        }

        // 4) �v���C���[�� carryPoint �̎q�ɂ��違�ʒu���Z�b�g
        carryingObject.transform.SetParent(carryPoint);
        carryingObject.transform.localPosition = Vector3.zero;
        carryingObject.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// �͂�ł���I�u�W�F�N�g���u������v����
    /// </summary>
    private void ThrowObject()
    {
        // 1) �`���[�W���Ԃ����Ƃɋ��x���v�Z
        float t = Mathf.Clamp01(grabChargeTimer / maxChargeTime);
        float force = Mathf.Lerp(minThrowForce, maxThrowForce, t);

        // 2) �����Ă���I�u�W�F�N�g�� carryPoint ����O��
        carryingObject.transform.SetParent(null);

        if (carryRb != null)
        {
            // 3) ���������𕜊�������
            carryRb.isKinematic = false;
            carryRb.useGravity = true;

            // 4) �u������ꂽ�v��Ԃ� PickableObject �ɓ`����
            var pickable = carryingObject.GetComponent<PickableObject>();
            if (pickable != null)
            {
                pickable.SetThrown(true);
            }

            // 5) Forward �x�N�g�� (�v���C���[�̌���) �ɉ����Ĕ�΂�
            Vector3 throwDir = transform.forward;
            carryRb.AddForce(throwDir * force, ForceMode.Impulse);
        }

        // 6) �����Ă����Ԃ��N���A
        carryingObject = null;
        carryRb = null;
        grabChargeTimer = 0f;
    }

    // �����M�Y���\���������ꍇ�͈ȉ���L����
    private void OnDrawGizmosSelected()
    {
        if (carryPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(grabPoint.position, grabRange);
        }
    }
}
