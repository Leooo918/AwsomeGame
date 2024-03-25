using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    private Vector3 moveDir;
    [SerializeField] private float moveSpeed = 3f;

    private void Update()
    {
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");

        Move();
    }

    private void Move()
    {
        moveDir = Quaternion.Euler(0, -45, 0) * moveDir * moveSpeed;
        transform.position += moveDir * Time.deltaTime;
    }
}
