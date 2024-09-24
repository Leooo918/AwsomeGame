using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileStatusEffect : StatusEffect
{
    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        _target.Stat.damageReceive.AddModifier((float)level / 100);
    }

    public override void OnEnd()
    {
        base.OnEnd();
        _target.Stat.damageReceive.RemoveModifier((float)level / 100);
    }
}
