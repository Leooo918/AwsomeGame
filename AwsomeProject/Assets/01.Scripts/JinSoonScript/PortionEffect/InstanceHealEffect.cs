using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceHealEffect : Effect
{
    [SerializeField]private int healAmount;

    public override void EnterEffort(Entity target)
    {
        target.healthCompo.GetHeal(healAmount);
    }
}
