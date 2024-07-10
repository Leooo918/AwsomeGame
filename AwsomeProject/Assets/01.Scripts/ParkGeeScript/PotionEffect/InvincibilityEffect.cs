using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityEffect : Effect
{
    private float _delay = 3f;

    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.Invincibility(3f);
        target.Stun(3f);
        target.StartDelayCallBack(_delay, () =>
        {
            target.InvincibilityDisable();
            Debug.Log("무적풀림");
        });
    }
}
