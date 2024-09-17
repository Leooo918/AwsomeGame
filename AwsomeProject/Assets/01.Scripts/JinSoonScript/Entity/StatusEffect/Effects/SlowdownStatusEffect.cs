using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownStatusEffect : StatusEffect
{
    private float[] _percentForLevel = { -0.1f, -0.2f, -0.3f };

    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        _target.Stat.moveSpeed.AddModifierByPercent(_percentForLevel[level]);
    }

    public override void OnEnd()
    {
        base.OnEnd();
        _target.Stat.moveSpeed.RemoveModifierByPercent(_percentForLevel[level]);
    }
}
