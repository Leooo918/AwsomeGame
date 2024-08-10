using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArmorEffect : Effect
{
    private float useTime = 5f;

    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.UpArmor(10);
        HasEffectManager.Instance.DamageOn(0);

        target.StartDelayCallBack(useTime, () =>
        {
            target.LostArmor(10);
            HasEffectManager.Instance.DamageOff();
        });
    }
}
