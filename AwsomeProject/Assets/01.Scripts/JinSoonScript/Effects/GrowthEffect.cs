using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        Debug.Log(target.name);
    }

    public override void ExitEffort()
    {
        base.ExitEffort();
    }

    public override void UpdateEffort()
    {
        base.UpdateEffort();
    }
}
