using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBornEffect : Effect
{
    //public PortionItemSO potionStat;
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        
        //if (potionStat == null)
        //{
        //    Debug.LogError("stat¿Ã «“¥Á æ»µ ");
        //    return;
        //}

        //duration = potionStat.duration;
        //figure = potionStat.figure;

        target.healthCompo.TakeDamage(10, Vector2.zero, null);
        target.AirBorn(1f);
    }
}
