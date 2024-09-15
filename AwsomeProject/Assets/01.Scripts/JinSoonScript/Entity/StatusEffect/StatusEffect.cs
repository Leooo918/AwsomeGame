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
        // 주석 돼 있는 이유 : 일단 StatusEffectType을 어디서 정해줘야 할지 모르겠음
        // 생성자 오버라이딩 느낌으로 추가해보려 했는데 이거 너무 노가다라 꼴봬기 싫음
        // 차라리 Buff, Debuff를 완전히 나누는것도 고려해봤는데 그러면 StatusEffectManager가 두개로 나뉘어서 보기 안좋음
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
