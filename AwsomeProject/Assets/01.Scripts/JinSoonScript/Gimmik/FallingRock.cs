using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable health))
            health.TakeDamage(1, Vector2.zero, null);

        //��ƼŬ �������ϰ�
        Destroy(gameObject);
    }
}
