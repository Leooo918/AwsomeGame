using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrificationStatusEffect : StatusEffect
{
    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);
        target.Stone(cooltime);
    }
}
