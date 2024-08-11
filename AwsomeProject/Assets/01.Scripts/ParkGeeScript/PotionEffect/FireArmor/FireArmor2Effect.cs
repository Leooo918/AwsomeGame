using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArmor2Effect : Effect
{
    private float useTime = 6f;

    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.UpArmor(20);
        HasEffectManager.Instance.DamageOn(0);

        target.StartDelayCallBack(useTime, () =>
        {
            target.LostArmor(20);
            HasEffectManager.Instance.DamageOff();
        });
    }
}
