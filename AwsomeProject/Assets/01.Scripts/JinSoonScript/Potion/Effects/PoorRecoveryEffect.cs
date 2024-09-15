using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoorRecoveryEffect : Effect
{
    public override void ApplyEffect()
    {
        foreach (var target in _affectedTargets)
        {
            if (target is Entity entity)
            {
                entity.healthCompo.TakeDamage(3, Vector2.zero, null);
                entity.ApplyStatusEffect(StatusDebuffEffectEnum.Petrification, _level, 3f);
            }
        }
    }
}
