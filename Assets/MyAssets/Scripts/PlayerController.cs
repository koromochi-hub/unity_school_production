using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 2.0f;
    private float rotationSpeed = 120.0f;

    // Update is called once per frame
    void Update()
    {
        // 垂直方向（上: 1、下: -1）
        float moveInput = Input.GetAxis("Vertical");

        // 水平方向（右: 1、左: -1）
        float rotateInput = Input.GetAxis("Horizontal");

        if(moveInput < 0)
        {
            rotateInput = -Input.GetAxis("Horizontal");
        }
             
        // 前進・後退の移動
        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);

        // 左右旋回
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.deltaTime);
    }
}
