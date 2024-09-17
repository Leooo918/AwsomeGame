using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoorRecoveryStatusEffect : StatusEffect
{
    private int[] _poorRecoveryForLevel = { -20, -30, -50 };

    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        _target.Stat.recoveryReceive.AddModifier(_poorRecoveryForLevel[level]);
    }

    public override void OnEnd()
    {
        base.OnEnd();
        _target.Stat.recoveryReceive.RemoveModifier(_poorRecoveryForLevel[level]);
    }
}
