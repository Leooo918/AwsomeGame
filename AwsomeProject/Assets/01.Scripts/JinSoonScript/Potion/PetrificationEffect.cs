using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrificationEffect : Effect
{
    public override void ApplyEffect()
    {
        foreach(var entity in _affectedTargets)
        {
            entity.healthCompo.TakeDamage(3, Vector2.zero, null);
            entity.Stun(2f);
        }
    }
}
