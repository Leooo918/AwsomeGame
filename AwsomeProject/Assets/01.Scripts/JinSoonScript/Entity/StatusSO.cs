using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommonStatType
{
    MaxHealth,
    Attack,
    AttackSpeed,
    MoveSpeed,
    GatheringSpeed,
    Defence
}

public abstract class StatusSO : ScriptableObject
{
    [Header("CommonSetting")]
    public Stat MaxHealth;
    public Stat Attack;
    public Stat AttackSpeed;
    public Stat MoveSpeed;
    public Stat GatheringSpeed;
    public Stat Defence;

    protected Entity owner;

    public Dictionary<CommonStatType, Stack> statDic;

    public virtual void SetOwner(Enemy owner)
    {
        this.owner = owner;
    }

    public void IncreaseStatBy(int value, float time, Stat stat)
    {
        owner.StartCoroutine(DelayReturnOriginValue(value, time, stat));                                                                                                                                                                                                                                   
    }

    private IEnumerator DelayReturnOriginValue(int value, float time, Stat stat)
    {
        stat.AddModifier(value);
        yield return new WaitForSeconds(time);
        stat.RemoveModifier(value);
    }


    public void CalculateDamage(ref int value)
    {
        Defence.GetValue();
        //방어력 연산을 이쪽에 써주면 됨
    }

}
