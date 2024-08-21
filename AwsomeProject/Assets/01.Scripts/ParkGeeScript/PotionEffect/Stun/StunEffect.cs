using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.healthCompo.TakeDamage(30, Vector2.zero, null);
        target.Stun(3f);
    }

    public override void UpdateEffort()
    {
        base.UpdateEffort();
    }

    public override void ExitEffort()
    {
        base.ExitEffort();
    }
}
