using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform headPos;      // 砲台(head)のTransform
    public LayerMask aimLayerMask;    // Rayがヒットするレイヤー（例：地面）

    void Update()
    {
        AimTurretAtMouse();
    }

    void AimTurretAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, aimLayerMask))
        {
            Vector3 targetPos = hit.point;
            Vector3 direction = targetPos - headPos.position;
            direction.y = 0f; // 水平方向だけで回転するように

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                headPos.rotation = Quaternion.Slerp(headPos.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
    }
}
