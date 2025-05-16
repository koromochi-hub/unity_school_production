using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform headPos;      // �C��(head)��Transform
    public LayerMask aimLayerMask;    // Ray���q�b�g���郌�C���[�i��F�n�ʁj

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
            direction.y = 0f; // �������������ŉ�]����悤��

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                headPos.rotation = Quaternion.Slerp(headPos.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
    }
}
