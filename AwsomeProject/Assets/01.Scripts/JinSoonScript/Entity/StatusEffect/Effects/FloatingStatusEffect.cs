using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingStatusEffect : StatusEffect
{
    private int[] damageForLevel = { 0, 10, 15 };

    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        target.AirBorn(cooltime, damageForLevel[level]);
    }
}
