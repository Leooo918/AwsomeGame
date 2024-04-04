using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactHit : MonoBehaviour
{
    [SerializeField] private int damage;
    private Entity owner;

    private void Awake()
    {
        owner = GetComponent<Entity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            Vector2 knockPower = (Vector2)(player.transform.position - transform.position).normalized + Vector2.up;
            knockPower *= 8;
            player.healthCompo.TakeDamage(damage, knockPower, owner);
        }
    }
}
