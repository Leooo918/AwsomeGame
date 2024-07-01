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
        Debug.Log("실행 스턴이펙트");
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
