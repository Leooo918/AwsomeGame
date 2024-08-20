using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpArmorEffect : Effect
{
    private float _delay = 5f;

    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.UpArmor(5);
        HasEffectManager.Instance.ArmorOn(0);
        target.StartDelayCallBack(_delay, () =>
        {
            target.LostArmor(5);
            HasEffectManager.Instance.ArmorOff();
        });
    }
}
