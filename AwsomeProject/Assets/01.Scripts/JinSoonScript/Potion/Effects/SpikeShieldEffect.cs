using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeShieldEffect : Effect
{
    public override void ApplyEffect()
    {
        foreach (var target in _affectedTargets)
        {
            if (target is Entity entity)
            {
                entity.ApplyStatusEffect(StatusBuffEffectEnum.SpikeShield, _level, 2);
            }
        }
    }
}
