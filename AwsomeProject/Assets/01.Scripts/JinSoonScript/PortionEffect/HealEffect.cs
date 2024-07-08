using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : Effect
{
    [SerializeField]private int healAmount;

    public override void EnterEffort(Entity target)
    {
        Debug.Log(target.healthCompo.curHp);
        target.healthCompo.GetHeal(5);
        Debug.Log(target.healthCompo.curHp);
    }
}
