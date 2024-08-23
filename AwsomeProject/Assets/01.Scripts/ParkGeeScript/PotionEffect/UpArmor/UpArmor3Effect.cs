using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpArmor3Effect : Effect
{
    private float _delay = 10f;

    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.UpArmor(10);
        HasEffectManager.Instance.ArmorOn(3);
        target.StartDelayCallBack(_delay, () =>
        {
            target.LostArmor(10);
            HasEffectManager.Instance.ArmorOff();
        });
    }
}
