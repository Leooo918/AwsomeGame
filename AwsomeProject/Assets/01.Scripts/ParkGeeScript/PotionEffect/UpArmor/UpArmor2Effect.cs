using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpArmor2Effect : Effect
{
    private float _delay = 10f;

    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.UpArmor(5);
        HasEffectManager.Instance.ArmorOn(1);
        target.StartDelayCallBack(_delay, () =>
        {
            target.LostArmor(5);
            HasEffectManager.Instance.ArmorOff();
        });
    }
}
