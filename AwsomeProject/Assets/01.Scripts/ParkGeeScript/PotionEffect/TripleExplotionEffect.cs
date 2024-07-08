using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleExplotionEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.healthCompo.TakeDamage(10, Vector2.zero, null);
        CoroutineManager.Instance.StartManagedCoroutine(Asd(target, 0.5f));
    }

    private IEnumerator Asd(Entity target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.healthCompo.TakeDamage(10, Vector2.zero, null);
        yield return new WaitForSeconds(delay);
        target.healthCompo.TakeDamage(10, Vector2.zero, null);
        target.Stun(1f);
    }
}
