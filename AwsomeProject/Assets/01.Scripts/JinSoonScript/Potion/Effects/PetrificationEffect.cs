using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrificationEffect : Effect
{
    public override void ApplyEffect()
    {
        foreach (var target in _affectedTargets)
        {
            if (target is Entity entity)
            {
                entity.ApplyStatusEffect(StatusDebuffEffectEnum.Petrification, _level, 3f);
            }
        }
    }
}
