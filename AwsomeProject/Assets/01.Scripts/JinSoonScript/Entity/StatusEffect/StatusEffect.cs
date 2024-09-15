using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    private float _startTime;
    private float _cooltime;
    public int level;

    public StatusEffect()
    {
        // �ּ� �� �ִ� ���� : �ϴ� StatusEffectType�� ��� ������� ���� �𸣰���
        // ������ �������̵� �������� �߰��غ��� �ߴµ� �̰� �ʹ� �밡�ٶ� �úı� ����
        // ���� Buff, Debuff�� ������ �����°͵� ����غôµ� �׷��� StatusEffectManager�� �ΰ��� ����� ���� ������
    }

    public void SetInfo(int level)
    {
        this.level = level;
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
