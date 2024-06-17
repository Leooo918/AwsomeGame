using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.healthCompo.TakeDamage(9999, Vector2.zero, null);
    }

    public override void ExitEffort()
    {
        base.ExitEffort();
    }

    public override void UpdateEffort()
    {
        base.UpdateEffort();
    }
}
