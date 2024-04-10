using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class Effect : MonoBehaviour
{
    protected Entity target;

    public float duration { get; protected set; } = 1f;
    public bool isInfiniteEffect { get; protected set; } = false;

    public abstract void EnterEffort(Entity target);

    public virtual void UpdateEffort()
    {

    }

    public virtual void ExitEffort()
    {

    }

    public void Init(float duration, bool isInfiniteEffect)
    {
        this.duration = duration;
        this.isInfiniteEffect = isInfiniteEffect;
    }
}
