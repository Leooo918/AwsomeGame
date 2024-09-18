using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedStatusEffect : StatusEffect
{
    private float[] _speedWithLevel = { 0.1f, 0.15f, 0.2f };

    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        _target.Stat.moveSpeed.AddModifierByPercent(_speedWithLevel[level]);
    }

    public override void OnEnd()
    {
        base.OnEnd();

        _target.Stat.moveSpeed.RemoveModifierByPercent(_speedWithLevel[level]);
    }
}
