using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBornEffect : Effect
{
    private int damage;
    PortionItemSO stat;

    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        float duration = stat.duration;
        target.healthCompo.TakeDamage(damage, Vector2.zero, null);
        target.AirBorn(duration);
    }
}
