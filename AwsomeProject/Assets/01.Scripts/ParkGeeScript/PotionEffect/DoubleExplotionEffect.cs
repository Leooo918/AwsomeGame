using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleExplotionEffect : Effect
{
    private float _delay = 0.5f;
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.healthCompo.TakeDamage(20, Vector2.zero, null);
        target.StartDelayCallBack(_delay, () =>
        {
            target.healthCompo.TakeDamage(20, Vector2.zero, null);
        });
    }
}
