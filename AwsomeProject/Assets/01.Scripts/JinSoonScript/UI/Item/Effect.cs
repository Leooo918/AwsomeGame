using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class Effect
{
    protected Entity target;

    public int portionLevel { get; protected set; } = 1;
    public float duration { get; protected set; } = 1f;

    public virtual void EnterEffort(Entity target)
    {

    }

    public virtual void UpdateEffort()
    {

    }

    public virtual void ExitEffort()
    {

    }

    public void Init(int level, float duration)
    {
        portionLevel = level;
        this.duration = duration;
    }
}
