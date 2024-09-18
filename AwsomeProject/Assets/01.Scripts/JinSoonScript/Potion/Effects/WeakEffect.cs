using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakEffect : Effect
{
    public override void ApplyEffect()
    {
        foreach (var target in _affectedTargets)
        {
            if (target is Entity entity)
            {
                entity.ApplyStatusEffect(StatusDebuffEffectEnum.Weak, _level, 5);
            }
        }
    }
}
