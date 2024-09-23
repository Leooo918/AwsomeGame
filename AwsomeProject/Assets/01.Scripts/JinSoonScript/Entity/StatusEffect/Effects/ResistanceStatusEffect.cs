using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceStatusEffect : StatusEffect
{
    private float[] _resistanceWithLevel = { -0.04f, -0.1f, -0.2f };

    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        _target.Stat.damageReceive.AddModifier(_resistanceWithLevel[level]);
    }

    public override void OnEnd()
    {
        base.OnEnd();

        _target.Stat.damageReceive.RemoveModifier(_resistanceWithLevel[level]);
    }
}
