using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpArmor4Effect : Effect
{
    private float _delay = 15f;

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
