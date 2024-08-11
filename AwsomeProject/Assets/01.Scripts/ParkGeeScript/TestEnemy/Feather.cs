using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour, IDamageable
{
    private Vector3 _direction;
    private float _speed;
    private float _maxLifeTime = 5f;
    private float _destroyingTime;
    private bool _stop = false;

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    private void Update()
    {
        if (_stop) return;

        transform.Translate(_direction * Time.deltaTime);

        if (_destroyingTime < Time.time)
            DestroyFeather();
    }

    private void DestroyFeather()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, Vector2 knockPower, Entity dealer)
    {
        Player player = dealer as Player;
        if (player == null) return;

        Vector2 direction = (transform.position - player.transform.position).normalized;
        float rot = Mathf.Atan2(direction.y, direction.x);
        _direction = direction * _speed;

        _stop = true;
        _destroyingTime = Time.time + _maxLifeTime;
        transform.DORotate(new Vector3(0, 0, rot), 0.1f)
            .OnComplete(() =>
            {
                _stop = false;

                });

    }

    public void Shoot(Vector2 playerDir)
    {
        float rot = Mathf.Atan2(playerDir.y, playerDir.x);
        transform.eulerAngles = new Vector3(0, 0, rot);

        _speed = playerDir.magnitude;
        _direction = playerDir;
        _destroyingTime = Time.time + _maxLifeTime;
    }
}
