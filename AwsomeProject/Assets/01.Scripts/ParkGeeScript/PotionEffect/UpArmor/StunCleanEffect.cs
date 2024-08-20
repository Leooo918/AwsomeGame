using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunCleanEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.Clean();
        Debug.Log("Á¤È­");
    }
}
