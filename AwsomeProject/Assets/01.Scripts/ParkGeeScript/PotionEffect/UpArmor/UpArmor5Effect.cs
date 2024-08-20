using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpArmor5Effect : Effect
{
    private float _delay = 20f;

    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.UpArmor(20);
        HasEffectManager.Instance.ArmorOn(0);
        target.StartDelayCallBack(_delay, () =>
        {
            target.LostArmor(20);
            HasEffectManager.Instance.ArmorOff();
        });
    }
}
