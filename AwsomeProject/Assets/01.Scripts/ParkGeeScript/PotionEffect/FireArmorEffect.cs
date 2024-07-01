using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArmorEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.UpArmor(10);
    }
}
