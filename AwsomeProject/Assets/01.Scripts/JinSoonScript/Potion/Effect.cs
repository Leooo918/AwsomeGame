using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    protected List<Entity> _affectedTargets;
    private Potion _potion;

    public void Initialize(Potion potion)
    {
        _potion = potion;
    }

    public void SetAffectedTargets(List<Entity> targets)
    {
        _affectedTargets = targets;
    }

    public abstract void ApplyEffect();
}