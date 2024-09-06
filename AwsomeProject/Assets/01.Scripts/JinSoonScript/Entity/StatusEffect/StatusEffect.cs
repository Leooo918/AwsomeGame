using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    private float _startTime;
    private float _cooltime;
    private StatusEffectInfo _info;

    public StatusEffect()
    {
        _info = new StatusEffectInfo();
    }

    public StatusEffectInfo GetInfo() => _info;

    public void SetInfo(int level)
    {
        _info.level = level;
    }

    public virtual void ApplyEffect(Entity target, float cooltime)
    {
        _startTime = Time.time;
        _cooltime = cooltime;
    }

    public virtual void UpdateEffect() { }
    public virtual void OnEnd() { }

    public bool IsCompleted() 
        =>_startTime + _cooltime < Time.time;
}
