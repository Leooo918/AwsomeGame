using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingEffect : Effect
{
    public override void ApplyEffect()
    {
        foreach (var target in _affectedTargets)
        {
            if (target is GrowingGrass)
            {
                target.ApplyEffect();
            }
        }
    }
}
