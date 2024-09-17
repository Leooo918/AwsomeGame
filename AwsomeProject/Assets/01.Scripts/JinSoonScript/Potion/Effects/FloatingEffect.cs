using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : Effect
{
    private float[] _durationForLevel = { 3, 4, 5 };

    public override void ApplyEffect()
    {
        foreach (var target in _affectedTargets)
        {
            if (target is Entity entity)
            {
                entity.ApplyStatusEffect(StatusDebuffEffectEnum.Floating, _level, _durationForLevel[_level]);
            }
        }
    }
}
