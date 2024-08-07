using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleExplotionEffect : Effect
{
    private float _delay = 0.5f;

    public override void EnterEffort(Entity target)
    {
        base.EnterEffort(target);
        target.healthCompo.TakeDamage(10, Vector2.zero, null);

        target.StartDelayCallBack(_delay, () =>
        {
            EffectInstantiateManager.Instance.ParticleInstantiate();
            target.healthCompo.TakeDamage(10, Vector2.zero, null);

            target.StartDelayCallBack(_delay, () =>
            {
                EffectInstantiateManager.Instance.ParticleInstantiate();
                target.healthCompo.TakeDamage(10, Vector2.zero, null);
                target.Stun(1f);
            });
        });
    }
}