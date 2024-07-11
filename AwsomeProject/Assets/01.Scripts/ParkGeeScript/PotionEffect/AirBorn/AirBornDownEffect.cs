using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBornDownEffect : Effect
{
    private float _delay = 1.6f;
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.healthCompo.TakeDamage(10, Vector2.zero, null);
        target.AirBorn(1.5f);
        target.StartDelayCallBack(_delay, () =>
        {
            target.healthCompo.TakeDamage(10, Vector2.zero, null);
        });
    }
}
