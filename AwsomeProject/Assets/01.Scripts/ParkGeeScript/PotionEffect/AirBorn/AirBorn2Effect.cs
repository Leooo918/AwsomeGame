using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBorn2Effect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);

        target.healthCompo.TakeDamage(10, Vector2.zero, null);
        target.AirBorn(1.5f);
    }
}
