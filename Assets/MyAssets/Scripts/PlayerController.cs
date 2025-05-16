using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float rotationSpeed = 120.0f;

    // Update is called once per frame
    void Update()
    {
        // ���������i��: 1�A��: -1�j
        float moveInput = Input.GetAxis("Vertical");

        // ���������i�E: 1�A��: -1�j
        float rotateInput = Input.GetAxis("Horizontal");

        // �O�i�E��ނ̈ړ�
        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);

        // ���E����
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.deltaTime);
    }
}
