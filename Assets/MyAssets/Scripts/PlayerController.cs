using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement data")]
    [SerializeField] private float moveSpeed; // 2.0f
    [SerializeField] private float rotationSpeed; // 120.0f

    [Header("Head data")]
    [SerializeField] private Transform headTransform;
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private float towerRotationSpeed; // 8.0f

    [Header("Shot data")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        TankMovement();
        UpdateAim();

        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        bullet.GetComponent<Rigidbody>().linearVelocity = gunPoint.forward * bulletSpeed;

        Destroy(bullet, 5);
    }

    private void TankMovement()
    {
        // 垂直方向（上: 1、下: -1）
        float moveInput = Input.GetAxis("Vertical");

        // 水平方向（右: 1、左: -1）
        float rotateInput = Input.GetAxis("Horizontal");

        if (moveInput < 0)
        {
            rotateInput = -Input.GetAxis("Horizontal");
        }

        // 前進・後退の移動
        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);

        // 左右旋回
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.deltaTime);
    }

    void UpdateAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, aimLayerMask))
        {
            Vector3 targetTransform = hit.point;
            Vector3 direction = targetTransform - headTransform.position;
            // 水平方向だけで回転するように
            direction.y = 0f; 

            // マウス位置が0に近いとき、無意味な回転などを防ぐ
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                headTransform.rotation = Quaternion.Slerp(headTransform.rotation, targetRotation, towerRotationSpeed * Time.deltaTime);
            }
        }
    }
}
