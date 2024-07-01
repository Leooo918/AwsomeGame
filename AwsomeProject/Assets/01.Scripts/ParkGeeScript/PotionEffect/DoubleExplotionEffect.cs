using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleExplotionEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.healthCompo.TakeDamage(20, Vector2.zero, null);
        CoroutineManager.Instance.StartManagedCoroutine(DelayBoom(target, 0.5f));
    }

    private IEnumerator DelayBoom(Entity target, float time)
    {
        yield return new WaitForSeconds(time);
        target.healthCompo.TakeDamage(20, Vector2.zero, null);
    }
}
