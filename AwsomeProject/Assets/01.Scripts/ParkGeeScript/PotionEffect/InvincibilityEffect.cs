using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.Invincibility(3f);
        target.Stun(3f);
        CoroutineManager.Instance.StartManagedCoroutine(DelayCoroutine(target, 3f));
    }

    private IEnumerator DelayCoroutine(Entity target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.InvincibilityDisable();
        Debug.Log("무적풀림");
    }
}
