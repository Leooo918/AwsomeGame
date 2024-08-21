using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun5Effect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.healthCompo.TakeDamage(50, Vector2.zero, null);
        target.Stun(3f);
        target.LostArmor(5);
        target.StartDelayCallBack(3f, () =>
        {
            target.UpArmor(5);
        });
    }
}
