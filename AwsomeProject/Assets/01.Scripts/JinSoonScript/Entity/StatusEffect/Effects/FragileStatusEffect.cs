using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileStatusEffect : StatusEffect
{
    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        _target.Stat.damageReceivPercent.AddModifier((float)level / 100);
    }

    public override void OnEnd()
    {
        base.OnEnd();
        _target.Stat.damageReceivPercent.RemoveModifier((float)level / 100);
    }
}
