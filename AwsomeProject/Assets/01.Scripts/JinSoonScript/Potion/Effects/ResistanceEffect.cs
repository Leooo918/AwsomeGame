using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceEffect : Effect
{
    public override void ApplyEffect()
    {
        foreach (var target in _affectedTargets)
        {
            if (target is Entity entity)
            {
                entity.ApplyStatusEffect(StatusBuffEffectEnum.Resistance, _level, 5);
            }
        }
    }
}
