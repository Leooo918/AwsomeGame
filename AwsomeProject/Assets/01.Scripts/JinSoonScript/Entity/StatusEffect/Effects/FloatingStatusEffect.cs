using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingStatusEffect : StatusEffect
{
    private int[] damageWithLevel = { 0, 10, 15 };

    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        Debug.Log("에어본");
        target.StartCoroutine(AirBornDurationCoroutine(cooltime, damageWithLevel[level]));
        target.AirBorn(cooltime);
    }

    IEnumerator AirBornDurationCoroutine(float duration, int damagePercent)
    {
        float elapsedTime = 0.0f;
        float initialVerticalSpeed = 5.0f;
        float originalXMovement = _target.MovementCompo.RigidbodyCompo.velocity.x;

        _target.MovementCompo.StopImmediately(true);
        float prevGravity = _target.MovementCompo.RigidbodyCompo.gravityScale;
        _target.MovementCompo.RigidbodyCompo.gravityScale = 0;

        while (elapsedTime < duration)
        {
            float verticalSpeed = 0;

            if (elapsedTime < 0.2f)
                verticalSpeed = initialVerticalSpeed * (1 - elapsedTime / duration);

            _target.MovementCompo.SetVelocity(new Vector2(0, verticalSpeed), withYVelocity: true);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _target.MovementCompo.RigidbodyCompo.gravityScale = prevGravity;

        float fallSpeed = -40f;
        _target.MovementCompo.SetVelocity(new Vector2(originalXMovement, fallSpeed), withYVelocity: true);
        yield return new WaitUntil(() =>
        {
            return _target.MovementCompo.RigidbodyCompo.velocity.y > fallSpeed;
        });
        _target.healthCompo.TakeDamage(damagePercent, Vector2.zero, owner, true);
        _target.MovementCompo.SetVelocity(new Vector2(originalXMovement, 0));

        Debug.Log("에어본 종료");
    }
}
