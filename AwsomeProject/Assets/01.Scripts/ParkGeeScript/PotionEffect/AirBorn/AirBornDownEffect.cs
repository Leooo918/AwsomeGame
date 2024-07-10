using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBornDownEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.healthCompo.TakeDamage(10, Vector2.zero, null);
        target.AirBorn(1.5f);
        CoroutineManager.Instance.StartManagedCoroutine(DownCoroutine(target));
    }

    private IEnumerator DownCoroutine(Entity target)
    {
        yield return new WaitForSeconds(1.6f);
        target.healthCompo.TakeDamage(10, Vector2.zero, null);
    }
}
