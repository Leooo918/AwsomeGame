using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakStatusEffect : StatusEffect
{
    private float[] _percentForLevel = { 10, 20, 30 };

    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        _target.Stat.damageReceive.AddModifier(_percentForLevel[level]);
    }

    public override void OnEnd()
    {
        base.OnEnd();
        _target.Stat.damageReceive.RemoveModifier(_percentForLevel[level]);
    }
}
