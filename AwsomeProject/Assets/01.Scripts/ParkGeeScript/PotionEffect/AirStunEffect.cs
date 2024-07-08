using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStunEffect : Effect
{
    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.AirBorn(1.5f);
        target.Stun(1.5f);
        CoroutineManager.Instance.StartManagedCoroutine(Asd(target, 1.6f));
    }

    private IEnumerator Asd(Entity target, float delay)
    {
        yield return new WaitForSeconds(delay);

        float radius = 1.0f;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(target.transform.position, radius);
        
        foreach (Collider2D hitCollider in hitColliders)
        {
            Entity hitEntity = hitCollider.GetComponent<Entity>();
            if (hitEntity != null)
            {
                hitEntity.healthCompo.TakeDamage(10, Vector2.zero, null);
            }
        }
        //InstantiateManager.Instance.Instantiate(target.transform);
    }
}
