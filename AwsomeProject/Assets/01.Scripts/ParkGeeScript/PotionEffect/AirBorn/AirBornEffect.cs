using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBornEffect : Effect // 지금 모든 효과 Effect가 1, 2, 3이런 식인데 potionStat에 뭐 만들어 둔거 있으니까 그거 활용해서 수정해야 함
{
    public PortionItemSO potionStat;
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        
        //if (potionStat == null)
        //{
        //    Debug.LogError("stat이 할당 안됨");
        //    return;
        //}

        //duration = potionStat.duration;
        //figure = potionStat.figure;

        target.healthCompo.TakeDamage(10, Vector2.zero, null);
        target.AirBorn(1f);
    }
}
