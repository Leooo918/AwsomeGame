using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    private float _startTime;
    private float _cooltime;
    private StatusEffectType _effectType;
    private StatusEffectInfo _info;

    public StatusEffect()
    {
        //_info = new StatusEffectInfo(_effectType);
        // �ּ� �� �ִ� ���� : �ϴ� StatusEffectType�� ��� ������� ���� �𸣰���
        // ������ �������̵� �������� �߰��غ��� �ߴµ� �̰� �ʹ� �밡�ٶ� �úı� ����
        // ���� Buff, Debuff�� ������ �����°͵� ����غôµ� �׷��� StatusEffectManager�� �ΰ��� ����� ���� ������
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
