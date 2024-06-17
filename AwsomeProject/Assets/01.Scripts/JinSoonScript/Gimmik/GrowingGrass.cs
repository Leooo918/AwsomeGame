using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingGrass : MonoBehaviour, IGetPortionEffect
{
    //private 
    private BoxCollider2D _collider;
    private Animator _animator;
    [SerializeField] private LayerMask _whatIsGround;

    private GrowingDirection _direction;
    private bool _isGrowing = false;
    private bool _isEndGrow = false;

    private float _currentScale;
    private float _growingSpeed = 9f;
    private float _maxGrowingSize = 7.5f;

    private int _growingAnimationHash = Animator.StringToHash("GrowStart");


    private void Awake()
    {
        _direction = (GrowingDirection)(int)(transform.eulerAngles.z / 90f);
        _animator = transform.Find("Visual").GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _currentScale = _collider.size.y;
    }

    private void Update()
    {
        Vector2 rayDir = transform.up;
        float distance = _maxGrowingSize;
        //rayDir = Quaternion.Euler(0, 0, 90 * (int)_direction) * rayDir;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, 15, _whatIsGround);
        if (hit.collider != null)
        {
            distance = hit.distance;
            _maxGrowingSize = distance;
        }
        Debug.DrawRay(transform.position, rayDir * distance);


        if (_isGrowing == false) return;

        _collider.size = new Vector2(0.76f, _currentScale);
        _collider.offset = new Vector2(0f, _currentScale / 2);
        _currentScale += Time.deltaTime * _growingSpeed;

        if (_currentScale > _maxGrowingSize)
        {
            _animator.enabled = false;
            _isGrowing = false;
            _isEndGrow = true;
        }
    }

    public void GetEffort(Effect effect)
    {
        GrowthEffect growth = effect as GrowthEffect;
        if (growth == null || _isEndGrow) return;

        _animator.SetTrigger(_growingAnimationHash);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.TryGetComponent(out IDamageable enemy) && _isGrowing)
        {
            enemy.Rape(3f);
            enemy.TakeDamage(0, Vector2.zero, null);
            //여기에 적 속박 시키게 하는 코드
        }

        //땅에 닿았다면 자라나는 걸 멈추게 해야함
        if (collision.CompareTag("Ground"))
        {
            _isEndGrow = true;
            _isGrowing = false;
            _collider.enabled = false;
        }
    }

    public void GrowStart()
    {
        _isGrowing = true;

        Vector2 rayDir = Vector2.up;
        rayDir = Quaternion.Euler(0, 0, 90 * (int)_direction) * rayDir;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, 15, _whatIsGround);
        Debug.DrawRay(transform.position, rayDir * 15);

        if (hit.collider != null) _maxGrowingSize = hit.distance + 0.8f;
    }
}

public enum GrowingDirection
{
    Up = 0,
    Left = 1,
    Down = 2,
    Right = 3,
}