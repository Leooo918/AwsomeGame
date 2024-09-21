using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour, IDamageable
{
    private float _speed;
    private float _maxLifeTime = 5f;
    private float _destroyingTime;
    private bool _stop = false;
    private bool _isStuck = false;

    [Range(-0.2f, 0.2f)]
    [SerializeField] private float _yOffset;
    [SerializeField] private LayerMask _whatIsObstacle;  
    [SerializeField] private LayerMask _whatIsTarget;  
    [SerializeField] private bool _onDebug;  


    private void FixedUpdate()
    {
        if (_stop) return;

        if (_isStuck == false)
        {
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(transform.position + transform.up * _yOffset, transform.up, _speed * Time.fixedDeltaTime, _whatIsTarget))
            {
                hit.transform.GetComponent<Player>().healthCompo.TakeDamage(1, Vector2.zero, null);
                CameraManager.Instance.ShakeCam(3f, 8f, 0.1f);
                DestroyFeather();
            }
            else if (hit = Physics2D.Raycast(transform.position + transform.up * _yOffset, transform.up, _speed * Time.fixedDeltaTime, _whatIsObstacle))
            {
                transform.Translate(Vector2.up * hit.distance);
                _isStuck = true;
            }
            else
            {
                transform.Translate(Vector2.up * _speed * Time.fixedDeltaTime);
            }
        }

        if (_destroyingTime < Time.time)
            DestroyFeather();
    }

    private void DestroyFeather()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, Vector2 knockPower, Entity dealer, bool isPersent = false)
    {
        if (_isStuck) return;

        Player player = dealer as Player;
        if (player == null) return;

        Vector2 direction = (transform.position - player.transform.position).normalized;


        CameraManager.Instance.ShakeCam(2f, 5f, 0.05f);

        _stop = true;
        _destroyingTime = Time.time + _maxLifeTime;
        transform.DORotateQuaternion(Quaternion.LookRotation(Vector3.back, direction), 0.03f)
            .OnComplete(() => _stop = false);

    }

    public void Shoot(Vector2 playerDir)
    {
        transform.up = playerDir;
        _speed = playerDir.magnitude;
        _destroyingTime = Time.time + _maxLifeTime;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_onDebug == false) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.up * _yOffset, 0.03f);
    }
#endif
}
