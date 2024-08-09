using UnityEngine;

public class EnemyContactHit : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Vector2 _knockBackPower;
    private Entity _owner;


    private void Awake()
    {
        _owner = GetComponentInParent<Entity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            Vector2 knockPower = _knockBackPower;
            if (player.transform.position.x > transform.position.x) knockPower.x *= -1;

            player.healthCompo.TakeDamage(_damage, knockPower, _owner);
        }
    }
}
