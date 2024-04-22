using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage, Vector2 knockPower, Entity dealer);
}