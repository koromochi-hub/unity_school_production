using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionPlayer : MonoBehaviour
{
    [SerializeField]private Animator anim;
    private Vector3 _velocity;

    public void OnMove(InputAction.CallbackContext context)
    {
        // MoveActionの入力値を取得
        var axis = context.ReadValue<Vector2>();

        // 移動速度を保持
        _velocity = new Vector3(axis.x, 0, axis.y);

        bool isRunning = _velocity.sqrMagnitude > 0.01f;
        anim.SetBool("isRunning", isRunning);

        if (_velocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_velocity);
    }

    private void Update()
    {
        // オブジェクト移動
        transform.position += _velocity * Time.deltaTime;
    }
}
