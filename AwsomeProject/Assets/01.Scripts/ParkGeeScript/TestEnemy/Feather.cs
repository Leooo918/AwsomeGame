using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private Vector3 _direction;

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    void Update()
    {
        transform.Translate(_direction * _moveSpeed * Time.deltaTime);
    }
}
