using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionPlayer : MonoBehaviour
{
    [SerializeField]private Animator anim;
    private Vector3 _velocity;

    public void OnMove(InputAction.CallbackContext context)
    {
        // MoveAction�̓��͒l���擾
        var axis = context.ReadValue<Vector2>();

        // �ړ����x��ێ�
        _velocity = new Vector3(axis.x, 0, axis.y);

        bool isRunning = _velocity.sqrMagnitude > 0.01f;
        anim.SetBool("isRunning", isRunning);

        if (_velocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_velocity);
    }

    private void Update()
    {
        // �I�u�W�F�N�g�ړ�
        transform.position += _velocity * Time.deltaTime;
    }
}
