using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileStatusEffect : StatusEffect
{
    private float[] _percentWithLevel = { 0.1f, 0.2f, 0.3f };

    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        _target.Stat.damageReceive.AddModifier(_percentWithLevel[level]);
    }

    public override void OnEnd()
    {
        base.OnEnd();
        _target.Stat.damageReceive.RemoveModifier(_percentWithLevel[level]);
    }
}
